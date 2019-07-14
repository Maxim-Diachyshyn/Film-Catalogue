import { gql } from "apollo-boost";

const todosQuery = gql`
query($status: TodoStatus){
    todos(status:$status){
      id
      name
      status
      createdAt
      assignedUser {
        id
        username
        email
        picture
      }
    }
}`;

const currentUserQuery = gql`
query {
    currentUser {
      id
      username
      email
      picture
      logoutRequested
      menuOpened
    }
}`;

export { todosQuery, currentUserQuery };
