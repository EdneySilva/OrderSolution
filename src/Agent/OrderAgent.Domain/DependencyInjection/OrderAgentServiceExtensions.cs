using OrderSolution.Core.Commands;
using Microsoft.Extensions.DependencyInjection;
using OrderAgent.Domain.Commands;
using OrderAgent.Domain.Handlers.Orders;

namespace OrderAgent.Domain.DependencyInjection
{
    public static class OrderAgentServiceExtensions
    {
        public static IServiceCollection AddAgentDomainServices(this IServiceCollection services)
        {
            services.AddSingleton<IAgent, Agent>();
            services.AddSingleton<IHandler<CreateOrderCommand>, OrderHandler>();
            return services;
        }
    }
}
