using PATOA.WebAPI.Attributes;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace PATOA.WebAPI.DI.Installers;

public static class SwaggerInstaller
{
    public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("admin", new OpenApiInfo
            {
                Title = "API PATOA",
                Version = "v1",
                Description = "API PATOA"
            });

           

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });

            // Inclure les commentaires XML
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        });

        return services;
    }

    public static WebApplication UseSwaggerServices(this WebApplication app)
    {

        app.UseSwagger();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/admin/swagger.json", "API Administration v1");
                c.RoutePrefix = "in";
                c.DocumentTitle = "Documentation API";
                c.DefaultModelsExpandDepth(-1);
            });
        }

       
        return app;
    }
} 