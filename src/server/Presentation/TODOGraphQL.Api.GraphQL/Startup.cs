﻿using Autofac;
using TODOGraphQL.Api.GraphQL.Schemas;
using TODOGraphQL.Persistence.EntityFramework.Contexts.Todos;
using GraphQL;
using GraphQL.Server;
using GraphQL.Server.Ui.GraphiQL;
using GraphQL.Server.Ui.Playground;
using GraphQL.Server.Ui.Voyager;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Google.Apis.Auth.OAuth2;
using GraphQL.Validation.Complexity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using GraphQL.Authorization;
using GraphQL.Validation;

namespace TODOGraphQL.Api.GraphQL
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDependencyResolver>(s => new FuncDependencyResolver(s.GetRequiredService));

            services.AddGraphQL(options =>
            {
                options.EnableMetrics = true;
                options.ExposeExceptions = Environment.IsDevelopment();
                // TODO: use this for security
                options.ComplexityConfiguration = new ComplexityConfiguration
                {
                    MaxDepth = 20,
                    MaxComplexity = 60,
                    FieldImpact = 2
                };
                options.SetFieldMiddleware = false;
            })
            // .AddUserContextBuilder(httpContext => new { httpContext.User })
            .AddWebSockets() // Add required services for web socket support
            .AddDataLoader(); // Add required services for DataLoader support

            services.AddHttpContextAccessor();
            services.AddCors();

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection = 
                        Configuration.GetSection("Authentication:Google");

                    options.ClientId = googleAuthNSection["ClientId"];
                    options.ClientSecret = googleAuthNSection["ClientSecret"];
                });
            

            // TODO: just check if authorized 
            services.TryAddSingleton(s =>
            {
                var authSettings = new AuthorizationSettings();

                authSettings.AddPolicy("User", _ => _.RequireClaim("User"));

                return authSettings;
            });

            services.TryAddSingleton<IAuthorizationEvaluator, AuthorizationEvaluator>();
            services.AddTransient<IValidationRule, AuthorizationValidationRule>();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new Persistence.Module());
            builder.RegisterModule(new Module());

            builder.Register(ctx => Configuration.GetSection("Authentication:Google").Get<ClientSecrets>())
                .AsSelf()
                .InstancePerDependency();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, TodoDbContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseGraphQLPlayground(new GraphQLPlaygroundOptions
                {
                    Path = "/ui/playground",
                    GraphQLEndPoint = "/graphql"
                });
                app.UseGraphiQLServer(new GraphiQLOptions
                {
                    GraphiQLPath = "/ui/graphiql",
                    GraphQLEndPoint = "/graphql"
                });
                app.UseGraphQLVoyager(new GraphQLVoyagerOptions
                {
                    Path = "/ui/voyager",
                    GraphQLEndPoint = "/graphql"
                });
            }
            else 
            {
                app.UseHsts();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
            });

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseCors(options => 
            {
                options.AllowAnyOrigin();
                options.AllowAnyHeader();
                options.AllowAnyMethod();
            });

            app.UseWebSockets();
            app.UseGraphQLWebSockets<TodoSchema>("/graphql");
            
            app.UseGraphQL<TodoSchema>("/graphql");

            app.UseStaticFiles();

            app.Use(async (ctx, next) =>
            {
                ctx.Request.Path = "/index.html";
                await next();
            }); 

            app.UseStaticFiles();

            context.Database.EnsureCreated();
        }
    }
}
