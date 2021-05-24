using OrderSolution.Core.Commands;
using System;

namespace OrderSupervisor.Domain.Commands
{
    public class CreateOrderCommand : ICommand
    {
        public CreateOrderCommand()
        {
            Random random = new Random();
            this.OrderId = Guid.NewGuid();
            this.RandomNumber = random.Next(1, 10);
        }

        public Guid OrderId { get; private set; }
        public int RandomNumber { get; private set; }
        public string OrderText { get; set; }

    }
}
