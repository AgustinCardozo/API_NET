using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace API_Demo.Services.Configs
{
    public static class SwaggerConfigService
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(service =>
            {
                service.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "DEMO.API",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "Agustin Cardozo",
                        Email = "agustincardozo8@yahoo.com",
                        Url = new Uri("https://github.com/AgustinCardozo/API_NET")
                    }
                });
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Description = "Put your JWT Bearer token",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                service.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                service.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
            });
        }

        public static void UseSwaggerConfig(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "DEMO.API v1");
                c.DefaultModelsExpandDepth(-1); //Oculta los Schemas de SwaggerUI
                c.ConfigObject.AdditionalItems.Add("syntaxHighlight", false);
            });
        }
    }
}
