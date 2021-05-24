using Azure.Storage.Queues;
using OrderSolution.Infrastructure.Storage.Abstractions.Configuration;
using OrderSolution.Infrastructure.Storage.Azure.Environment;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace OrderSolution.Infrastructure.Storage.Azure.Configuration
{
    class AzureQueueSetupProvider : IEnvironmentSetupProvider
    {
        private readonly ILogger<AzureQueueSetupProvider> logger;
        private readonly IOptions<AzureConnectionOptions> options;

        public AzureQueueSetupProvider(ILogger<AzureQueueSetupProvider> logger, IOptions<AzureConnectionOptions> options)
        {
            this.logger = logger;
            this.options = options;
        }

        public void Apply()
        {
            QueueClient queueClient = new QueueClient(this.options.Value.ConnectionString, this.options.Value.Queue);
            logger.LogDebug($"Check if the queue {options.Value.Queue} exists, keep calm it will be created if not exists =)");
            var response = queueClient.CreateIfNotExists();
            if (queueClient.Exists())
            {
                logger.LogDebug($"The {options.Value.Queue} exists");
            }
            else
            {
                logger.LogCritical($"Ops! something is wrong, the queue {options.Value.Queue} was not found, it can causes unexpected behaviours!");
                return;
            }
            logger.LogDebug($"The service {nameof(AzureStorageEnvironmentConfiguration)} has been  started");
        }
    }
}
