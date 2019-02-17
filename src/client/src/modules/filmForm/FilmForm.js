import React, { Component } from 'react';
import { Mutation, withApollo } from "react-apollo";
import _ from "lodash";
import { Link } from 'react-router-dom'
import {editMutation, createMutation} from "./mutation";
import filmQuery from "./query";
import filmsQuery from "../films/query";
import ROUTES from "../appRouter/routes";
import moment from 'moment';

class EditForm extends Component {
    constructor(props){
        super(props);

        this.state = {
            film: {
                name: "",
                id: "",
                showedDate: moment().format("YYYY-MM-DD"),
                photo: ""
            },
            isUploadingFile: false
        };
    }

    async componentDidMount() {
        if (!this.props.isAddingForm) {
            let film = _.get(this.props.location, "state.film", null);
            if (!film) {
                const response = await this.props.client.query({query: filmQuery, variables: {id: this.props.match.params.id}});
                film = {
                    ...response.data.film,
                    showedDate: moment(response.data.film.showedDate).format("YYYY-MM-DD")
                };
            }
            this.setState({film});
        }
    }

    _onNameChange = (e) => {
        const name = e.target.value;
        this.setState(prevState => ({film:{...prevState.film, name}}));
    }

    _onShowedDateChange = (e) => {
        const showedDate = e.target.value;
        this.setState(prevState => ({film:{...prevState.film, showedDate}}));
    }

    _onfileChange = (e) => {
        var reader = new FileReader();
        const file = e.target.files[0];
        if (!file) {
            return;
        }
        reader.readAsDataURL(file);
        this.setState({isUploadingFile: true, photo: ""});
        reader.onload = () => {
            this.setState(prevState => ({isUploadingFile: false, film:{...prevState.film, photo: reader.result}}));
        };
    }

    withMutation = handler => {
        const { history, isAddingForm } = this.props;
        const mutation = isAddingForm 
            ? createMutation
            : editMutation;
        const variables = isAddingForm 
            ? _.pick(this.state.film, ["name", "showedDate", "photo"])
            : _.pick(this.state.film, ["id", "name", "showedDate", "photo"]);

        const update = isAddingForm ?
            (store, { data: { createFilm } }) => {
                if (store.data.data.ROOT_QUERY) {
                    const data = store.readQuery({ query: filmsQuery });
                    data.films.push(createFilm);
                    store.writeQuery({ 
                        query: filmsQuery,
                        data
                    });
                }
                
                history.push(ROUTES.HOME)
            }
            : () => history.push(ROUTES.HOME);
        return (
            <Mutation
                mutation={mutation}
                variables={variables}
                update={update}
            >
                {isAddingForm
                    ? (createFilm, {loading, error}) => handler(createFilm, {loading, error})
                    : (editFilm, {loading, error}) => handler(editFilm, {loading, error})
                }
            </Mutation>
        );
    }

    render() {
        const { isAddingForm } = this.props;
        const { film, isUploadingFile } = this.state;
        return this.withMutation((mutation, {loading, error}) => {
            const errors = _.get(error, "graphQLErrors", []);
            if (loading) {
                if (isAddingForm) {
                    return <span>Adding New Film...</span>
                }
                else {
                    return <span>Updating Film...</span>
                }
            }
            return (
                <div style={containerStyle}>                
                    <Link to={ROUTES.HOME} style={backButtonStyle}>Back</Link>
                    <form onSubmit={e => {e.preventDefault(); mutation();}} style={formContainerStyle}>
                        <span>Film Name:</span>
                        <div style={inputContainerStyle}>
                            <input
                                type='text'
                                value={film.name}
                                onChange={this._onNameChange}
                            />
                            {_.some(errors, e => e.extensions.code === "EmptyName") 
                                ? (
                                    <span style={errorStyle}>Film name should not be empty</span>
                                )
                                : null}
                        </div>

                        <span>Premiere Date:</span>
                        <div style={inputContainerStyle}>
                            <input
                                type='date'
                                value={film.showedDate}
                                onChange={this._onShowedDateChange}
                            />
                            {_.some(errors, e => e.extensions.code === "EmptyDate") 
                            ? (
                                <span style={errorStyle}>Showed date should not be empty</span>
                            )
                            : null}
                        </div>

                        <span>Photo:</span>
                        <input 
                            type="file"
                            onChange={this._onfileChange}
                        />
                        <img src={film.photo} style={photoStyle} height={300} width={300}/>
                        <button type='submit' disabled={isUploadingFile} style={submitStyle}>Ok</button>
                    </form>
                </div>
            )
        });
    }
}

const containerStyle = {
    display: "flex",
    flexDirection: "column",
    padding: 20
}

const errorStyle = {
    color: "red",
    fontSize: 12
}

const inputContainerStyle = {
    display: "flex",
    flexDirection: "column"
}

const backButtonStyle = {
    alignSelf: "flex-start",
}

const formContainerStyle = {
    padding: 20,
    display: "grid",
    maxWidth: 450,
    justifyContent: "space-between",
    textAlign: "left",
    gridTemplateColumns: "1fr 1fr",
    gridColumnGap: 10,
    gridRowGap: 10,
    alignSelf: "center",
}

const photoStyle = {
    gridColumn: "1 / span 2",
}

const submitStyle = {
    gridColumn: "1 / span 2",
}
  
export default withApollo(EditForm);
