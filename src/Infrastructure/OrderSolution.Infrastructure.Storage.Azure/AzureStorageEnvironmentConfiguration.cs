using OrderSolution.Infrastructure.Storage.Abstractions;
using OrderSolution.Infrastructure.Storage.Abstractions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace OrderSolution.Infrastructure.Storage.Azure
{
    public class AzureStorageEnvironmentConfiguration : IStorageEnvironmentConfiguration
    {
        private readonly ILogger<AzureStorageEnvironmentConfiguration> logger;
        private readonly IEnumerable<IEnvironmentSetupProvider> environmentSetupProviders;

        public AzureStorageEnvironmentConfiguration(ILogger<AzureStorageEnvironmentConfiguration> logger, IEnumerable<IEnvironmentSetupProvider> environmentSetupProviders)
        {
            this.logger = logger;
            this.environmentSetupProviders = environmentSetupProviders;
        }

        public void Initialize()
        {
            logger.LogDebug($"The service {nameof(AzureStorageEnvironmentConfiguration)} is starting");
            foreach (var item in this.environmentSetupProviders)
            {
                try
                {
                    item.Apply();
                }
                catch (System.Exception ex)
                {
                    logger.LogError(ex, $"Ops! something is wrong, trying to apply the setup {item.GetType().Name}:\n" + ex.Message);
                }
            }
            logger.LogDebug($"The service {nameof(AzureStorageEnvironmentConfiguration)} has been  started");
        }
    }
}
