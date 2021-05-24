using OrderSolution.Infrastructure.Storage.Abstractions.Configuration;
using OrderSolution.Infrastructure.Storage.Azure.Environment;
using OrderSolution.Infrastructure.Storage.Azure.Tables.Schemma;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;

namespace OrderSolution.Infrastructure.Storage.Azure.Configuration
{
    class AzureTableSetupProvider : IEnvironmentSetupProvider
    {
        private readonly ILogger<AzureTableSetupProvider> logger;
        private readonly ObjectPool<CloudTableClient> defaultObjectPool;
        private readonly IOptions<AzureConnectionOptions> options;
        private readonly string tableName;

        public AzureTableSetupProvider(ILogger<AzureTableSetupProvider> logger, IOptions<AzureConnectionOptions> options, ObjectPoolProvider objectPoolProvider, IPooledObjectPolicy<CloudTableClient> defaultObjectPool, TableSchemma tableSchemma)
        {
            this.logger = logger;
            this.defaultObjectPool = objectPoolProvider.Create(defaultObjectPool);
            this.options = options;
            this.tableName = tableSchemma.TableName;
        }

        public void Apply()
        {
            var client = this.defaultObjectPool.Get();
            try
            {
                logger.LogDebug($"Check if the table {options.Value.Queue} exists, keep calm it will be created if not exists =)");
                CloudTable table = client.GetTableReference(tableName);
                if (table.CreateIfNotExists() || table.Exists())
                {
                    logger.LogDebug($"The table {table.Name} exists");
                }
                else
                {
                    logger.LogCritical($"Ops! something is wrong, the table {table.Name} was not found, it can causes unexpected behaviours!");
                    return;
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
            finally
            {
                this.defaultObjectPool.Return(client);
            }
        }
    }
}
