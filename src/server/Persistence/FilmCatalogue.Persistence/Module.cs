﻿using System;
using System.Linq;
using System.Reactive.Subjects;
using Autofac;
using Autofac.Core;
using FilmCatalogue.Domain.UseCases.Films.Models;
using FilmCatalogue.Domain.UseCases.Reviews.Models;
using FilmCatalogue.Persistence.EntityFramework.Base;
using FilmCatalogue.Persistence.EntityFramework.Contexts.Reviews.Builders;
using FilmCatalogue.Persistence.Notification.Contexts.Films;
using FilmCatalogue.Persistence.Notification.Contexts.Reviews;
using FluentValidation;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Pipeline;

namespace FilmCatalogue.Persistence
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var interfacesToRegister = new[] {
                typeof(IRequestHandler<,>),
                typeof(IValidator<>),
                typeof(IRequestPostProcessor<,>),
                typeof(IQueryBuilder<,>)
            };

            builder.RegisterType<ReviewsQueryBuilder>().AsImplementedInterfaces();

            foreach (var interfaceToRegister in interfacesToRegister)
            {
                builder.RegisterAssemblyTypes(ThisAssembly)
                    .Where(type => type.GetInterfaces()
                    .Where(i => i.IsGenericType)
                    .Select(i => i.GetGenericTypeDefinition())
                    .Contains(interfaceToRegister))
                    .AsSelf()
                    .As(interfaceToRegister);
            }

            builder.AddMediatR(ThisAssembly);
            builder.RegisterModule(new EntityFramework.Module());

            var filmAddedStream = new ReplaySubject<Film>(0);
            builder.RegisterType<FilmAddedHandler>()
                .AsSelf()
                .AsImplementedInterfaces()
                .WithParameter(
                    new ResolvedParameter(
                        (pi, ctx) => pi.ParameterType == typeof(ISubject<Film>) && pi.Name == "filmStream",
                        (pi, ctx) => filmAddedStream
                    )
                )
                .SingleInstance();

            var filmUpdatedStream = new ReplaySubject<Film>(0);
            builder.RegisterType<FilmUpdatedHandler>()
                .AsSelf()
                .AsImplementedInterfaces()
                .WithParameter(
                    new ResolvedParameter(
                        (pi, ctx) => pi.ParameterType == typeof(ISubject<Film>) && pi.Name == "filmStream",
                        (pi, ctx) => filmUpdatedStream
                    )
                )
                .SingleInstance();

            var filmRemovedStream = new ReplaySubject<Film>(0);
            builder.RegisterType<FilmRemovedHandler>()
                .AsSelf()
                .AsImplementedInterfaces()
                .WithParameter(
                    new ResolvedParameter(
                        (pi, ctx) => pi.ParameterType == typeof(ISubject<Film>) && pi.Name == "filmStream",
                        (pi, ctx) => filmRemovedStream
                    )
                )
                .SingleInstance();

            var reviewAddedStream = new ReplaySubject<Review>(0);
            builder.RegisterType<ReviewAddedHandler>()
            .AsSelf()
                .AsImplementedInterfaces()
                .WithParameter(
                    new ResolvedParameter(
                        (pi, ctx) => pi.ParameterType == typeof(ISubject<Review>) && pi.Name == "reviewStream",
                        (pi, ctx) => reviewAddedStream
                    )
                )
                .SingleInstance();
        }
    }
}
