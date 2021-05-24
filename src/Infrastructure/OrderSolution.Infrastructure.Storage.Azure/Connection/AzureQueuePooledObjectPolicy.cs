using Azure.Storage.Queues;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace OrderSolution.Infrastructure.Storage.Azure.Connection
{
    public class AzureQueuePooledObjectPolicy : IPooledObjectPolicy<QueueClient>, IAsyncDisposable
    {
        private readonly Environment.AzureConnectionOptions azureConnectionOptions;

        public AzureQueuePooledObjectPolicy(IOptions<Environment.AzureConnectionOptions> options)
        {
            azureConnectionOptions = options.Value;
        }

        public QueueClient Create()
        {
            QueueClient queueClient = new QueueClient(azureConnectionOptions.ConnectionString, azureConnectionOptions.Queue);

            return queueClient;
        }

        public ValueTask DisposeAsync()
        {
            return new ValueTask(Task.CompletedTask);
        }

        public bool Return(QueueClient obj)
        {
            return true;
        }
    }
}
