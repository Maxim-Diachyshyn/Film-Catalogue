﻿using System.Linq;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using FilmCatalogue.Persistence.EntityFramework.Contexts.Films.Requests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FilmCatalogue.Persistence.EntityFramework
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            var services = new ServiceCollection();

            services.AddDbContextPool<FilmDbContext>(options =>
            {
                // options.UseSqlServer("Data Source=\".\";Initial Catalog=Films;Integrated Security=False;User ID=sa;Password=Password1;MultipleActiveResultSets=true");
                options.UseInMemoryDatabase("TestDatabase");
            });

            builder.Populate(services);

            builder.RegisterSource(new QueryableRegistrationSource<FilmDbContext>());
        }
    }
}
