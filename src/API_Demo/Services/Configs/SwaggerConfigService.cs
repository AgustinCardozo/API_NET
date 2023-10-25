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
                service.SwaggerDoc(Consts.Version.V1, GetOpenApiInfo(Consts.Version.V1));
                service.SwaggerDoc(Consts.Version.V2, GetOpenApiInfo(Consts.Version.V2));
                var jwtSecurityScheme = GetOpenApiSecurityScheme();
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
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "DEMO.API v2");
                c.DefaultModelsExpandDepth(-1); //Oculta los Schemas de SwaggerUI
                c.ConfigObject.AdditionalItems.Add("syntaxHighlight", false);
            });
        }

        private static OpenApiInfo GetOpenApiInfo(string version)
        {
            return new OpenApiInfo
            {
                Title = "DEMO.API",
                Version = version,
                Contact = new OpenApiContact
                {
                    Name = "Agustin Cardozo",
                    Email = "agustincardozo8@yahoo.com",
                    Url = new Uri("https://github.com/AgustinCardozo/API_NET")
                }
            };
        }

        private static OpenApiSecurityScheme GetOpenApiSecurityScheme()
        {
            return new OpenApiSecurityScheme
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
        }
    }
}
