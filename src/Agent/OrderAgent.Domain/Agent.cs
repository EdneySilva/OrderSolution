using OrderSolution.Core.Commands;
using OrderSolution.Infrastructure.Storage.Abstractions.Communication;
using OrderSolution.Infrastructure.Storage.Abstractions.Text;
using Microsoft.Extensions.Logging;
using OrderAgent.Domain.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderAgent.Domain
{
    public class Agent : IAgent
    {
        private readonly ILogger<Agent> logger;
        private readonly IBus bus;
        private readonly IHandler<CreateOrderCommand> handler;

        public Agent(ILogger<Agent> logger, IBus bus, IHandler<CreateOrderCommand> handler)
        {
            this.AgentId = Guid.NewGuid();
            GenerateMagicNumber();
            this.logger = logger;
            this.bus = bus;
            this.handler = handler;
        }

        public Guid AgentId { get; }
        public int MagicNumber { get; private set; }

        public void GenerateMagicNumber()
        {
            var random = new Random();
            this.MagicNumber = random.Next(1, 10);
        }

        public Task Execute(CancellationToken stoppingToken)
        {
            logger.LogInformation($"I`m agent {this.AgentId}, my magic number is {this.MagicNumber}");
            return Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    Message busMessage = null;
                    try
                    {
                        busMessage = await bus.TakeMessageAsync(stoppingToken);
                        var order = busMessage.To<OrderSolution.Core.Order>();
                        logger.LogInformation($"Received order {order.OrderId}");
                        if (order.RandomNumber.Equals(this.MagicNumber))
                        {
                            logger.LogCritical("Oh no, my magic number was found");
                            break;
                        }
#if DEBUG
                        await Task.Delay(5000);
#endif
                        await handler.Handle(new CreateOrderCommand(order.OrderId, order.RandomNumber, order.OrderText, this.AgentId));
                        await busMessage.ConfirmAsync();
                    }
                    catch(Exception ex)
                    {
                        this.logger.LogError(ex, "An exception occurred when try to take a message from the queue: \n" + ex.Message);
                    }                 
                }
            });
        }
    }
}