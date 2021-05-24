using Azure.Storage.Queues;
using OrderSolution.Core.Commands;
using OrderSolution.Infrastructure.Storage.Abstractions.Communication;
using OrderSolution.Infrastructure.Storage.Abstractions.Text;
using OrderSolution.Infrastructure.Storage.Azure.Environment;
using OrderSolution.Infrastructure.Storage.Azure.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace OrderSolution.Infrastructure.Storage.Azure.Communication
{
    class AzureQueueBus : IBus
    {
        private readonly ObjectPool<QueueClient> defaultObjectPool;
        private readonly AzureConnectionOptions azureConnectionOptions;
        private readonly ILogger<AzureQueueBus> logger;

        public AzureQueueBus(IOptions<AzureConnectionOptions> options, ObjectPoolProvider objectPoolProvider, IPooledObjectPolicy<QueueClient> defaultObjectPool, ILogger<AzureQueueBus> logger)
        {
            this.defaultObjectPool = objectPoolProvider.Create(defaultObjectPool);
            this.azureConnectionOptions = options.Value;
            this.logger = logger;
        }

        public Task SendCommandAsync<T>(T command) where T : ICommand
        {
            var queueClient = defaultObjectPool.Get();
            try
            {
                var messageContent = JsonSerializer.Serialize(command);
                var result = queueClient.SendMessage(messageContent);
                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An exception occurred when try to send the command: \n" + ex.Message);
                return Task.CompletedTask;
            }
            finally
            {
                this.defaultObjectPool.Return(queueClient);
            }
        }

        public Task<Message> TakeMessageAsync(CancellationToken cancellationToken)
        {
            return Task.Run(async () =>
            {
                var queueClient = defaultObjectPool.Get();
                try
                {
                    Message azureMessage = null;
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        var message = await queueClient.ReceiveMessageAsync();
                        if(message.Value == null)
                        {
                            await Task.Delay(azureConnectionOptions.RequestWaitInterval ?? 1000);
                            continue;
                        }
                        azureMessage = new AzureMessage(message, queueClient);
                        break;
                    }
                    return azureMessage;
                }
                catch (Exception ex)
                {
                    throw;
                }
                finally
                {
                    this.defaultObjectPool.Return(queueClient);
                }
            });
        }
    }
}
