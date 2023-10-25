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

        serviceCollection.AddScoped<IConfiguration>(x =>
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

            configurationBuilder
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("testsettings.json");

            return configurationBuilder.Build();
        });

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
        serviceCollection.AddLogging();

        return serviceCollection;
    }

    public T GetRequiredService<T>()
    {
        var provider = Services().BuildServiceProvider();
        return provider.GetRequiredService<T>();
    }
}