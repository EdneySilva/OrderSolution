using OrderSolution.Core;
using OrderSolution.Infrastructure.Storage.Abstractions.Communication;
using OrderSolution.Infrastructure.Storage.Azure.Environment;
using OrderSolution.Infrastructure.Storage.Azure.Tables.Schemma;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderSolution.Infrastructure.Storage.Azure.Communication
{
    class AzureTableReplyMessage : IBusMessageReply
    {
        private readonly ILogger<AzureTableReplyMessage> logger;
        private readonly AzureConnectionOptions azureConnectionOptions;
        private readonly string tableName;
        private ObjectPool<CloudTableClient> defaultObjectPool;

        public AzureTableReplyMessage(
            ObjectPoolProvider objectPoolProvider,
            IPooledObjectPolicy<CloudTableClient> defaultObjectPool,
            ILogger<AzureTableReplyMessage> logger,
            TableSchemma tableSchemma,
            IOptions<AzureConnectionOptions> azureConnectionOptions)
        {
            this.defaultObjectPool = objectPoolProvider.Create(defaultObjectPool);
            this.logger = logger;
            this.azureConnectionOptions = azureConnectionOptions.Value;
            this.tableName = tableSchemma.TableName;
        }

        public async Task SendConfirmation(Confirmation confirmation)
        {
            var tableClient = defaultObjectPool.Get();
            try
            {
                var table = tableClient.GetTableReference(tableName);
                TableOperation tableOperation = TableOperation.Insert(new Tables.Entities.Confirmation(confirmation.OrderId, confirmation.AgentId, confirmation.OrderStatus));
                TableResult result = await table.ExecuteAsync(tableOperation);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An exception occurred when try to send the command: \n" + ex.Message);
            }
            finally
            {
                this.defaultObjectPool.Return(tableClient);
            }
        }

        public Task<Confirmation> WaitResponseAsync(string key, TimeSpan timeout)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource(timeout);
            var timeoutOperation = new TimeoutOperation<Confirmation>(timeout, CreateWaitTask(cancellationTokenSource.Token, key), cancellationTokenSource);
            return timeoutOperation.Wait();
        }

        private Task<Confirmation> CreateWaitTask(CancellationToken cancellationToken, string key)
        {
            var mainTask = Task.Run(async () =>
            {
                var tableClient = defaultObjectPool.Get();
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var table = tableClient.GetTableReference(tableName);
                        TableOperation tableOperation = TableOperation.Retrieve<Tables.Entities.Confirmation>(key, key);
                        TableResult result = await table.ExecuteAsync(tableOperation);
                        if (result.HttpStatusCode != 200)
                        {
                            await Task.Delay(this.azureConnectionOptions.RequestWaitInterval ?? 1000);
                            continue;
                        }
                        Tables.Entities.Confirmation confirmation = result.Result as Tables.Entities.Confirmation;
                        return new Confirmation
                        {
                            AgentId = confirmation.AgentId,
                            OrderId = confirmation.OrderId,
                            OrderStatus = confirmation.OrderStatus
                        };
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An exception occurred when try to send the command: \n" + ex.Message);
                        throw ex;
                    }
                    finally
                    {
                        this.defaultObjectPool.Return(tableClient);
                    }
                }
                return null;
            });
            return mainTask;
        }
    }
}
