using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace OrderSolution.Infrastructure.Storage.Azure.Connection
{
    public class AzureTablePooledObjectPolicy : IPooledObjectPolicy<CloudTableClient>, IAsyncDisposable
    {
        private readonly Environment.AzureConnectionOptions azureConnectionOptions;

        public AzureTablePooledObjectPolicy(IOptions<Environment.AzureConnectionOptions> options)
        {
            azureConnectionOptions = options.Value;
        }

        public CloudTableClient Create()
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(this.azureConnectionOptions.ConnectionString);
            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient(new TableClientConfiguration());
            return cloudTableClient;
        }

        public ValueTask DisposeAsync()
        {
            return new ValueTask(Task.CompletedTask);
        }

        public bool Return(CloudTableClient obj)
        {
            return true;
        }
    }
}
