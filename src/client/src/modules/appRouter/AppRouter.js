import React, { Component } from 'react';
import { Switch, Route, BrowserRouter, Redirect } from 'react-router-dom';
import ROUTES from "./routes";
import { components as boardComponents } from "../Board";

class AppRouter extends Component {
    render() {
      return (
        <BrowserRouter>
          <Switch>
            <Route exact path={ROUTES.HOME} component={boardComponents.default}/>
            <Route exact path={ROUTES.EDIT_FILM.route} component={boardComponents.default}/>
            <Redirect to={ROUTES.HOME} />
          </Switch>
        </BrowserRouter>
      );
    }
  }
  
  export default AppRouter;
