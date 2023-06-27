using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RtuItLab.Infrastructure.Filters;
using RtuItLab.Infrastructure.Middlewares;
using Shops.DAL.Data;
using Shops.Domain.Services;
using System.Collections.Generic;
using RtuItLab.Infrastructure.MassTransit.Configuration;

namespace Shops.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(option =>
            {
                option.Filters.Add(typeof(ValidateModelAttribute));
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "Shops Service",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id   = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name   = "Bearer",
                            In     = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
            services.AddDbContext<ShopsDbContext>(
                option => option.UseInMemoryDatabase("DefaultConnection"), ServiceLifetime.Transient);
            services.AddScoped<IShopsService, ShopsService>();
            services.Configure<RabbitMqConfiguration>(Configuration.GetSection(nameof(RabbitMqConfiguration)));

            services.AddMassTransit(x =>
            {
                // Shops
                x.AddConsumers(typeof(Startup));
                
                x.UsingRabbitMq((context, cfg) =>
                {
                    var configuration = Configuration.GetSection(nameof(RabbitMqConfiguration))
                        .Get<RabbitMqConfiguration>();
                    
                    cfg.Host(configuration.Host, "/", h =>
                    {
                        h.Username(configuration.Username);
                        h.Password(configuration.Password);
                    });
                });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

            app.UseSwagger()
                .UseSwaggerUI(c =>
              {
                  c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shops.API V1");
              });
            app.UseRouting();
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<JwtMiddleware>();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}