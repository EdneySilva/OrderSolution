using Azure.Storage.Queues;
using OrderSolution.Infrastructure.Storage.Abstractions;
using OrderSolution.Infrastructure.Storage.Abstractions.Communication;
using OrderSolution.Infrastructure.Storage.Abstractions.Configuration;
using OrderSolution.Infrastructure.Storage.Azure.Communication;
using OrderSolution.Infrastructure.Storage.Azure.Configuration;
using OrderSolution.Infrastructure.Storage.Azure.Connection;
using OrderSolution.Infrastructure.Storage.Azure.Environment;
using OrderSolution.Infrastructure.Storage.Azure.Tables.Schemma;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.ObjectPool;

namespace OrderSolution.Infrastructure.Storage.Azure.DependencyInjection
{
    public static class AzureStorageServiceExtensions
    {
        public static IServiceCollection AddAzureAsStorageService(this IServiceCollection services, IConfigurationSection azureConnectionSection, string confirmationTableName)
        {
            services.AddOptions();
            services.Configure<AzureConnectionOptions>(azureConnectionSection);
            services.AddSingleton<IStorageEnvironmentConfiguration, AzureStorageEnvironmentConfiguration>();
            services.AddSingleton<IPooledObjectPolicy<QueueClient>, AzureQueuePooledObjectPolicy>();
            services.AddSingleton<IPooledObjectPolicy<CloudTableClient>, AzureTablePooledObjectPolicy>();
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<IBus, AzureQueueBus>();
            services.AddSingleton<IBusMessageReply, AzureTableReplyMessage>();
            services.AddSingleton(new TableSchemma(confirmationTableName));
            return services;
        }

        public static IServiceCollection AddAzureQueueSetup(this IServiceCollection services)
        {
            services.AddSingleton<IEnvironmentSetupProvider, AzureQueueSetupProvider>();
            return services;
        }

        public static IServiceCollection AddAzureTableSetup(this IServiceCollection services)
        {
            services.AddSingleton<IEnvironmentSetupProvider, AzureTableSetupProvider>();
            return services;
        }
    }
}
