using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace NetExtensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwashbuckle(this IServiceCollection services, string title = null, string description = null, string version = null) =>
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(version ?? "v1", new OpenApiInfo { Title = title ?? "Api", Version = version ?? "v1", Description = description ?? "" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
                c.EnableAnnotations();
            });

        public static IApplicationBuilder AddSwashbuckle(this IApplicationBuilder app, string name = null, string endpoint = null)
        {
            app.UseSwagger();
            return app.UseSwaggerUI(c => { c.SwaggerEndpoint(endpoint ?? "/swagger/v1/swagger.json", name ?? "API"); });
        }
    }
}