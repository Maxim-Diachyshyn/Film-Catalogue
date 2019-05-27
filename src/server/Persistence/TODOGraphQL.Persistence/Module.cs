﻿using System;
using System.Linq;
using Autofac;
using TODOGraphQL.Persistence.EntityFramework.Base;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Pipeline;
using System.Reactive.Subjects;
using TODOGraphQL.Persistence.ReactiveExtensions;
using TODOGraphQL.Application.UseCases.Todos.Commands;
using TODOGraphQL.Domain.DataTypes.Todos;

namespace TODOGraphQL.Persistence
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var interfacesToRegister = new[] {
                typeof(IRequestHandler<,>),
                typeof(IRequestPostProcessor<,>),
                typeof(IQueryBuilder<,>)
            };

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

            builder.RegisterGeneric(typeof(ReplaySubject<>))
                .WithParameter("bufferSize", 0)
                .As(typeof(ISubject<>));

            builder.RegisterType<GenericObservable<AddTodosCommand, Todo>>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();
            builder.RegisterType<GenericObservable<UpdateTodosCommand, Todo>>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();
            builder.RegisterType<GenericObservable<DeleteTodosCommand, Todo>>()
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();
        }
    }
}
