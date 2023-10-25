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
using API_Demo.Helpers.Filters;
using API_Demo.Services.Configs;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

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
            var logger = LoggerConfigService.GetLogger();

            string environment = Environment.GetEnvironmentVariable(Consts.StartupConfig.ENVIRONMENT);
            logger.LogInformation($"ENVIRONMENT: {environment}", DateTimeOffset.Now);
            string conn = Configuration.GetConnectionString(Consts.ConfigKeys.CONN_DB);
            string connLog = !conn.Contains("Integrated Security") ? PasswordHelper.HideConnectionString(conn) : conn;
            logger.LogInformation($"ConnStr: {connLog}");

            services.AddLogging();
            services.AddSingleton(typeof(ILogger), typeof(Logger<Startup>));

            services.AddControllers(config =>
            {
                config.Filters.Add(typeof(ExceptionsFilter));
                config.Filters.Add(typeof(RequestsFilter));
            });
            services.AddCors(policy =>
            {
                policy.AddDefaultPolicy(options => options.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
            });
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader
                    .Combine(new UrlSegmentApiVersionReader(),
                            new HeaderApiVersionReader("x-api-version"),
                            new MediaTypeApiVersionReader("x-api-version")
                    );
            });

            // this is needed to work AddApiVersioning
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;   
            });
            services.AddSingleton<DapperContext>();
            services.AddScoped<IValidator<ClienteReq>, ClienteValidator>();
            services.AddScoped<IValidator<RegistrarUsuarioReq>, UsuarioValidator>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IProductoRepository, ProductoRepository>();
            services.AddScoped<ILogginService, LogginService>();

            AuthenticationConfigService.AddAuthenticationConfiguration(services, Configuration[Consts.StartupConfig.JWT_KEY]);   
            SwaggerConfigService.AddSwaggerConfiguration(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // Para que funcione en test o prod, tiene que ir fuera del if
                SwaggerConfigService.UseSwaggerConfig(app);
            }

            app.UseCors(options =>
            {
                options
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
