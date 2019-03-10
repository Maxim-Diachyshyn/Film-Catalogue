﻿using FilmCatalogue.Api.GraphQL.GraphTypes;
using FilmCatalogue.Domain.DataTypes;
using FilmCatalogue.Domain.UseCases.Film.Models;
using FilmCatalogue.Domain.UseCases.Film.Requests;
using GraphQL;
using GraphQL.Types;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FilmCatalogue.Api.GraphQL.Queries
{
    public class Query : ObjectGraphType
    {
        public Query(IMediator mediator)
        {
            Name = "query";
            Field<ListGraphType<FilmType>, IEnumerable<FilmModel>>()
                .Name("films")
                .ResolveAsync(async context =>
                {
                    return await mediator.Send(new GetFilmListRequest());
                });

            Field<FilmType, FilmModel>()
                .Name("film")
                .Argument<NonNullGraphType<IdGraphType>, Guid>("id", "Film id.")
                .ResolveAsync(async context =>
                {
                    var id = context.GetArgument<Guid>("id");
                    var films = await mediator.Send(new GetFilmListRequest(new Id(id)));
                    var film = films.SingleOrDefault();
                    if (film == null)
                    {
                        context.Errors.Add(new ExecutionError("Not found") {Code="NotFound"});
                    }
                    return film;
                });
        }
    }
}
