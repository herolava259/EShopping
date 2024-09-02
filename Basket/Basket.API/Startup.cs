﻿using Basket.API.Swagger;
using Basket.Application.GrpcService;
using Basket.Application.Handlers;
using Basket.Core.Repositories;
using Basket.Infrastructure.Repositories;
using Common.Logging.Correlation;
using Discount.Grpc.Protos;
using HealthChecks.UI.Client;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Basket.API
{
    public class Startup
    {
        public IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {

            this.Configuration = configuration;
            
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                        new HeaderApiVersionReader("X-Version"),
                        new QueryStringApiVersionReader("api-version", "ver"),
                        new MediaTypeApiVersionReader("ver")
                    );
                /*options.ApiVersionReader = new HeaderApiVersionReader("X-Version");
                options.ApiVersionReader = new MediaTypeApiVersionReader("ver");
                options.ApiVersionReader = new QueryStringApiVersionReader("api-version", "ver");*/
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                });
            });
            //.AddVersionedApiExplorer(
            //    options =>
            //    {
            //        options.GroupNameFormat = "'v'VVV";
            //        options.SubstituteApiVersionInUrl = true;
            //    });

            // Redis Settings
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetValue<string>("CacheSettings:ConnectionString");

            });

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(CreateShoppingCartCommandHandler).GetTypeInfo().Assembly));

            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<DiscountGrpcService>();
            services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
                (o => o.Address = new Uri(Configuration["GrpcSettings:DiscountUrl"]));

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();
            });
            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("v1", new OpenApiInfo
            //    {
            //        Title = "Basket.API",
            //        Version = "v1"
            //    });
            //});

            services.AddHealthChecks()
                    .AddRedis(Configuration["CacheSettings:ConnectionString"]!, "Redis Health", HealthStatus.Degraded);
            services.AddMassTransit(config =>
            {
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    /*cfg.ConfigureEndpoints(ctx);
                    cfg.Host("amqp://guest:guest@localhost:15672", h =>
                    {
                        h.Username(Configuration["EventBusSettings:UserName"]);
                        h.Password(Configuration["EventBusSettings:Password"]);
                    });*/
                    cfg.Host(Configuration["EventBusSettings:HostAddress"]);

                });
            });
            services.AddMassTransitHostedService();

            //Identity Server changes
            //var userPolicy = new AuthorizationPolicyBuilder()
            //                        .RequireAuthenticatedUser()
            //                        .Build();
            //services.AddControllers(config => config.Filters.Add(new AuthorizeFilter(userPolicy)));
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //        .AddJwtBearer(options =>
            //        {
            //            options.Authority = "https://id-local.eshopping.com:44344";
            //            options.Audience = "Basket";
            //        });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            var nginxPath = "/basket";
            if(env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                { 
                    foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
                    {
                        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                            description.GroupName.ToUpperInvariant());
                        //options.RoutePrefix = String.Empty;
                    }

                    //options.DocumentTitle = "Basket API Documentation";
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("CorsPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });
        }
    }
}
