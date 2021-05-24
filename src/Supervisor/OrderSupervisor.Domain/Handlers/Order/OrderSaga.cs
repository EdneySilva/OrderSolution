using OrderSolution.Core.Commands;
using OrderSolution.Infrastructure.Storage.Abstractions.Communication;
using Microsoft.Extensions.Logging;
using OrderSupervisor.Domain.Commands;
using OrderSupervisor.Domain.Services;
using System;
using System.Threading.Tasks;

namespace OrderSupervisor.Domain.Handlers.Order
{
    public class OrderSaga : IHandler<CreateOrderCommand>
    {
        private readonly IBus eventBus;
        private readonly ICounterService counterService;
        private readonly ILogger<OrderSaga> logger;
        private readonly IBusMessageReply busMessageReply;

        public OrderSaga(IBus eventBus, ICounterService counterService, ILogger<OrderSaga> logger, IBusMessageReply busMessageReply)
        {
            this.eventBus = eventBus;
            this.counterService = counterService;
            this.logger = logger;
            this.busMessageReply = busMessageReply;
        }

        public async Task<ICommandResult> Handle(CreateOrderCommand command)
        {
            var result = await counterService.NextValueAsync();
            logger.LogDebug($"Created orders #{result}");
            await eventBus.SendCommandAsync(command);
            logger.LogInformation($"Send order #{command.OrderId} with random number RandomNumber");
            var response = await busMessageReply.WaitResponseAsync(command.OrderId.ToString(), TimeSpan.FromSeconds(30));
            return new OrderSolution.Core.Commands.Results.SuccessResult
            {
                Data = response
            };
        }
    }
}
