﻿using Autofac;
using FilmCatalogue.Api.GraphQL.GraphTypes;
using FilmCatalogue.Api.GraphQL.Inputs;
using FilmCatalogue.Api.GraphQL.Mutations;
using FilmCatalogue.Api.GraphQL.Queries;
using FilmCatalogue.Api.GraphQL.Schemas;
using FilmCatalogue.Api.GraphQL.Subscriptions;
using GraphQL.Types;

namespace FilmCatalogue.Api.GraphQL
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            
            builder.RegisterType<IdGraphType>().SingleInstance();

            builder.RegisterType<FilmType>().SingleInstance();
            builder.RegisterType<AddFilmInputType>().SingleInstance();
            builder.RegisterType<UpdateFilmInputType>().SingleInstance();

            builder.RegisterType<ReviewType>().SingleInstance();
            builder.RegisterType<AddReviewInputType>().SingleInstance();

            builder.RegisterType<Query>().SingleInstance();
            builder.RegisterType<Mutation>();
            builder.RegisterType<Subscription>().SingleInstance();

            builder.RegisterType<FilmSchema>().SingleInstance();

            builder.RegisterModule(new FilmCatalogue.Api.Common.Module());
        }
    }
}
