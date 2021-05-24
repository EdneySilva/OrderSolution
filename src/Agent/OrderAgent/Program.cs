using OrderSolution.Infrastructure.Storage.Azure.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderAgent.Domain.DependencyInjection;
using OrderAgent.Services;
using System;
using System.Threading.Tasks;

namespace OrderAgent
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureHostConfiguration(configuration =>
                {
                    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                    environment = string.IsNullOrEmpty(environment) ? "Development" : environment;
                    configuration.AddJsonFile($"appsettings.json", optional: false, reloadOnChange: true);
                    configuration.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
                });
            host.ConfigureServices((context, services) =>
            {
                services.AddAzureAsStorageService(context.Configuration.GetSection("AzureConnection"), "confirmations");
                services.AddAgentDomainServices();
                services
                    .AddAzureQueueSetup()
                    .AddAzureTableSetup();
                services.AddHostedService<BackGroundService>();
            });
            Task.WaitAll(host.RunConsoleAsync());
        }
    }
}
