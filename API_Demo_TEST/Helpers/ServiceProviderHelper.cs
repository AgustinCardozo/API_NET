using API_Demo.Configurations;
using API_Demo.Controllers;
using API_Demo.Database;
using API_Demo.Database.Repositories;
using API_Demo.Database.Repositories.Contracts;
using API_Demo.Models.Requests;
using API_Demo.Services;
using API_Demo.Services.Contracts;
using API_Demo.Validators;
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

            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("testsettings.json")
                .Build();

            serviceCollection.AddOptions()
                .Configure<ApiDemoOptions>(configuration.GetSection(ApiDemoOptions.Key))
                .Configure<ConnStrOptions>(configuration.GetSection(ConnStrOptions.Key));

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

            var serviceProvider = serviceCollection.BuildServiceProvider();

            return serviceProvider;
        }
    }
}
