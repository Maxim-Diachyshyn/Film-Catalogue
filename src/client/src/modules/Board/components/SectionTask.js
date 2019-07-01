import React from 'react';
import _ from "lodash";
import { ListItemSecondaryAction, ListItemText, IconButton, ListItemIcon, ListSubheader, ListItemAvatar, Avatar } from '@material-ui/core';
import { Delete as DeleteIcon, PersonAdd as PersonAddIcon } from '@material-ui/icons';
import { ListItem } from '@material-ui/core';
import { Mutation } from "react-apollo";
import { deleteTodo } from "../mutations";
import { TASK_STATUSES } from "../../Task/constants";

const styles = {
    link: {
        // borderRadius: 8,
        // margin: "2px 0px",
        background: "#FFFF",
        minHeight: 75
    },
    circle: {
        borderRadius: "50%",
        width: 40,
        height: 40
    },
    text: {
        overflow: "hidden",
        textOverflow: "ellipsis"
    },
    [`status${TASK_STATUSES.Open}`]: {
        background: "#bdbdbd",
        color: "white"
    },
    [`status${TASK_STATUSES["In Progress"]}`]: {
        background: "#03a9f4",
    },
    [`status${TASK_STATUSES.Review}`]: {
        background: "#9575cd"
    },
    [`status${TASK_STATUSES.Done}`]: {
        background: "#388e3c"
    },
    deleteButton: {
        padding: 5,
        color: "#f44336"
    }
};

const texts = {
    delete: "Delete"
};

const SectionTask = props => {
    const { name, status, onSelect, onDelete } = props;
    return (
        <ListItem button={true} style={styles.link} onClick={onSelect}>
            <ListItemAvatar style={styles[`status${status}`]}>
                {/* <Avatar src="https://sophosnews.files.wordpress.com/2014/04/anonymous-250.jpg?w=250"> */}
                {/* TODO: ternary operator with checcking if any user is assigned */}
                <Avatar>
                    <PersonAddIcon/>
                </Avatar>
            </ListItemAvatar>
            <ListItemText primary={<p style={styles.text}>{name}</p>} />
        </ListItem>
    )
};

export default props => {
    const { id } = props;
    return (
        <Mutation mutation={deleteTodo}>{(deleteTodo) => (
            <SectionTask {...props} onDelete={() => deleteTodo({ variables: { id } })} />
        )}</Mutation>
    );
}
    