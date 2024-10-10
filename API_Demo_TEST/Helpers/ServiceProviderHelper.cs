using API_Demo.Controllers;
using API_Demo.Database;
using API_Demo.Database.Repositories;
using API_Demo.Database.Repositories.Contracts;
using API_Demo.Models.Requests;
using API_Demo.Services;
using API_Demo.Services.Contracts;
using API_Demo.Validators;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using FluentValidation;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API_Demo_TEST.Helpers
{
    public class ServiceProviderHelper
    {
        private class EmptyStartup
        {
            public EmptyStartup(IConfiguration _) { }
            public void ConfigureServices(IServiceCollection _) { }
            public void Configure(IApplicationBuilder _) { }
        }

        public static ServiceProvider GenerateServiceProvider()
        {
            //Startup startup = null;
            IServiceCollection serviceCollection = null;
            WebHost
                .CreateDefaultBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.Sources.Clear();
                    config.AddConfiguration(hostingContext.Configuration);
                    config.AddJsonFile("testsettings.json");
                    //startup = new Startup(config.Build());
                })
                .ConfigureServices(sc =>
                {
                    //startup.ConfigureServices(sc);
                    serviceCollection = sc;
                })
                .UseStartup<EmptyStartup>()
                .Build();

            const string KEY = "Whosoever holds this hammer, if they be worthy, shall possess the power of Thor";

            serviceCollection.AddSingleton<DapperContext>();
            serviceCollection.AddTransient<IValidator<ClienteReq>, ClienteValidator>();
            serviceCollection.AddTransient<IClienteRepository, ClienteRepository>();
            serviceCollection.AddTransient<IUsuarioRepository, UsuarioRepository>();
            serviceCollection.AddTransient<CotizadorController>();
            serviceCollection.AddTransient<ClienteController>();
            serviceCollection.AddSingleton<IJwtTokenService>(new JwtTokenService(KEY));
            serviceCollection.AddTransient<ILogginService, LogginService>();
            serviceCollection.AddTransient<AuthController>();
            serviceCollection.AddTransient<IClienteService, ClienteService>();


            var serviceProvider = serviceCollection.BuildServiceProvider(new ServiceProviderOptions()
            {
                ValidateOnBuild = true,
                ValidateScopes = true
            });

            return serviceProvider;
        }
    }
}
