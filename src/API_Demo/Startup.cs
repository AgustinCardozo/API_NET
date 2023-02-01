//using Carter;
global using API_Demo.Database;
global using API_Demo.Database.Repositories;
global using API_Demo.Database.Repositories.Contracts;
global using API_Demo.Helpers;
global using API_Demo.Helpers.Exceptions;
global using API_Demo.Models.Requests;
global using API_Demo.Models.Responses;
global using API_Demo.Services;
global using API_Demo.Services.Contracts;
global using API_Demo.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Text;

namespace API_Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //net 6
            //using var loggerFactory = LoggerFactory.Create(builder =>
            //{
            //    builder.AddSimpleConsole(i => i.ColorBehavior = LoggerColorBehavior.Disabled);
            //});

            //var logger = loggerFactory.CreateLogger<Startup>();

            using var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Information);
                builder.AddConsole();
                builder.AddEventSourceLogger();
            });
            var logger = loggerFactory.CreateLogger("Startup");

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            logger.LogInformation($"ENVIRONMENT: {environment}", DateTimeOffset.Now);
            var conn = Configuration.GetConnectionString(Consts.ConfigKeys.CONN_DB);
            var connLog = !conn.Contains("Integrated Security") ? PasswordHelper.HideConnectionString(conn) : conn;
            logger.LogInformation($"ConnStr: {connLog}");

            //services.AddCarter();
            services.AddControllers();
            services.AddSingleton<DapperContext>();
            services.AddScoped<IValidator<ClienteReq>, ClienteValidator>();
            services.AddScoped<IValidator<RegistrarUsuarioReq>, UsuarioValidator>();
            services.AddTransient<IClienteRepository, ClienteRepository>();
            services.AddTransient<IUsuarioRepository, UsuarioRepository>();
            services.AddTransient<ILogginService, LogginService>();

            string key = Configuration["JWT:key"];

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = false
                };
            });
            services.AddAuthorization();
            services.AddSingleton<IJwtTokenService>(new JwtTokenService(key));

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
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
                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // Para que funcione en test o prod, tiene que ir fuera del if
                app.UseSwagger();
                app.UseSwaggerUI(c => { 
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DEMO.API v1");
                    c.DefaultModelsExpandDepth(-1); //Oculta los Schemas de SwaggerUI
                });
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapCarter();
                endpoints.MapControllers();
            });
        }
    }
}
