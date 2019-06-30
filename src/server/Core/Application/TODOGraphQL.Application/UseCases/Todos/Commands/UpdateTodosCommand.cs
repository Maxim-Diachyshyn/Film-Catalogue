﻿using TODOGraphQL.Domain.DataTypes.Common;
using TODOGraphQL.Domain.DataTypes.Todos;
using MediatR;
using System;
using System.Collections.Generic;

namespace TODOGraphQL.Application.UseCases.Todos.Commands
{
    public class UpdateTodosCommand : IRequest<IDictionary<Id, Todo>>
    {
        public IDictionary<Id, Todo> OldTodos { get; set; }
        public IDictionary<Id, Todo> Todos { get; set; }
    }
}
