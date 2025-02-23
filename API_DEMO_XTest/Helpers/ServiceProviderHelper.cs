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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace API_DEMO_XTest.Helpers;

public class ServiceProviderHelper
{
    public ServiceCollection Services()
    {
        var serviceCollection = new ServiceCollection();

        var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("testsettings.json")
                .Build();

        serviceCollection.AddScoped<IConfiguration>(x =>
        {
            return configuration;
        });

        serviceCollection.AddOptions()
                .Configure<ApiDemoOptions>(configuration.GetSection(ApiDemoOptions.Key))
                .Configure<ConnStrOptions>(configuration.GetSection(ConnStrOptions.Key));

        const string KEY = "Whosoever holds this hammer, if they be worthy, shall possess the power of Thor";

        serviceCollection.AddTransient<AuthController>();
        serviceCollection.AddTransient<ClienteController>();
        serviceCollection.AddTransient<CotizadorController>();
        serviceCollection.AddSingleton<DapperContext>();
        serviceCollection.AddTransient<IClienteService, ClienteService>();
        serviceCollection.AddSingleton<IJwtTokenService>(new JwtTokenService(KEY));
        serviceCollection.AddTransient<ILogginService, LogginService>();
        serviceCollection.AddTransient<IClienteRepository, ClienteRepository>();
        serviceCollection.AddTransient<IUsuarioRepository, UsuarioRepository>();
        serviceCollection.AddTransient<IValidator<ClienteReq>, ClienteValidator>();
        serviceCollection.AddLogging();

        return serviceCollection;
    }

    public T GetRequiredService<T>()
    {
        var provider = Services().BuildServiceProvider();
        return provider.GetRequiredService<T>();
    }
}