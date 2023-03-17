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
using API_Demo.Services.Configs;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
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

            services.AddControllers();
            services.AddSingleton<DapperContext>();
            services.AddScoped<IValidator<ClienteReq>, ClienteValidator>();
            services.AddScoped<IValidator<RegistrarUsuarioReq>, UsuarioValidator>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ILogginService, LogginService>();

            AuthenticationConfigService.AddAuthenticationConfiguration(services, Configuration[Consts.StartupConfig.JWT_KEY]);

            //environment = conn = connLog = null;
            //loggerFactory.Dispose();           

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

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {;
                endpoints.MapControllers();
            });
        }
    }
}
