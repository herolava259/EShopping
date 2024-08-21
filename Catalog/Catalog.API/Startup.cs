﻿using Catalog.Application.Commands;
using Catalog.Application.Handlers;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using HealthChecks.UI.Client;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Catalog.API
{
    public class Startup
    {
        public IConfiguration Configuration;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApiVersioning()
                    ;
            services
                    .AddCors(options =>
                    {
                        options.AddPolicy("CorsPolicy", policy =>
                        {
                            policy.AllowAnyHeader()
                                  .AllowAnyMethod()
                                  .AllowAnyOrigin();
                        });
                    })
                    .AddVersionedApiExplorer(
                    options =>
                    {
                        options.GroupNameFormat = "'v'VVV";
                        options.SubstituteApiVersionInUrl = true;
                    });
            services.AddHealthChecks()
                    .AddMongoDb(Configuration["DatabaseSettings:ConnectionString"], 
                                "Catalog Mongo Db Health Check", 
                                HealthStatus.Degraded);
            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Catalog.API", 
                    Version = "v1" });
            });
            //services.AddSwaggerDocumentation();
            //DI
            services.AddAutoMapper(typeof(Startup));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateProductHandler).GetTypeInfo().Assembly));
            //.AddMediatR(typeof(CreateProductHandler).GetTypeInfo().Assembly);
            services.AddScoped<ICatalogContext, CatalogContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBrandRepository, ProductRepository>();
            services.AddScoped<ITypesRepository, ProductRepository>();
            services.AddScoped<ICatalogContext, CatalogContext>();

            //Identity Server changes
            //var userPolicy = new AuthorizationPolicyBuilder()
            //                        .RequireAuthenticatedUser()
            //                        .Build();
            //services.AddControllers(config => config.Filters.Add(new AuthorizeFilter(userPolicy)));
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //        .AddJwtBearer(options =>
            //        {
            //            options.Authority = "https://id-local.eshopping.com:44344";
            //            options.Audience = "Catalog";
            //        });
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("CanRead", policy => policy.RequireClaim("scope", "catalogapi.read"));
            //});
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            var nginxPath = "catalog";
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
                app.UseSwagger();
                //app.UseSwaggerUI(options =>
                //{
                //    foreach (var description in provider.ApiVersionDescriptions)
                //    {
                //        options.SwaggerEndpoint($"{nginxPath}/swagger/{description.GroupName}/swagger.json",
                //            $"Catalog API {description.GroupName.ToUpperInvariant()}");
                //        options.RoutePrefix = String.Empty;
                //    }

                //    options.DocumentTitle = "Catalog API Documentation";
                //});
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.API v1"));
            }

            app.UseRouting();
            //app.UseAuthentication();
            app.UseStaticFiles();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
