using OrderSolution.Core.Commands;
using System;

namespace OrderAgent.Domain.Commands
{
    public class CreateOrderCommand : ICommand
    {
        public CreateOrderCommand(Guid orderId, int randomNumber, string orderText, Guid agentId)
        {
            OrderId = orderId;
            RandomNumber = randomNumber;
            OrderText = orderText;
            AgentId = agentId;
        }

        public Guid OrderId { get; set; }
        public int RandomNumber { get; set; }
        public string OrderText { get; set;  }
        public Guid AgentId { get; set; }
    }
}
