using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using OrderSolution.Infrastructure.Storage.Abstractions.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OrderSolution.Infrastructure.Storage.Azure.Text
{
    class AzureMessage : Message
    {
        private readonly QueueMessage queueMessage;
        private readonly QueueClient queueClient;

        public AzureMessage(QueueMessage queueMessage, QueueClient queueClient)
            : base(queueMessage.MessageText)
        {
            this.queueMessage = queueMessage;
            this.queueClient = queueClient;
        }

        public override T To<T>()
        {
            return JsonSerializer.Deserialize<T>(this.Content);
        }

        public override Task ConfirmAsync()
        {
            return queueClient.DeleteMessageAsync(queueMessage.MessageId, queueMessage.PopReceipt);
        }
    }
}
