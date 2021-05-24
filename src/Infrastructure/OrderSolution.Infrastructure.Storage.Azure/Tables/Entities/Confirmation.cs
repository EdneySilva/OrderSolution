using Microsoft.Azure.Cosmos.Table;
using System;

namespace OrderSolution.Infrastructure.Storage.Azure.Tables.Entities
{
    public class Confirmation : TableEntity
    {
        public Confirmation()
        {
        }

        public Confirmation(Guid orderId, Guid agentId, string orderStatus)
        {
            OrderId = orderId;
            AgentId = agentId;
            OrderStatus = orderStatus;
            this.PartitionKey = orderId.ToString();
            this.RowKey = orderId.ToString();
        }

        public Guid OrderId { get; set; }
        public Guid AgentId { get; set; }
        public string OrderStatus { get; set; }
    }
}
