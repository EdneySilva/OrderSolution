using OrderSolution.Core.Commands;
using Microsoft.Extensions.DependencyInjection;
using OrderSupervisor.Domain.Commands;
using OrderSupervisor.Domain.Services;

namespace OrderSupervisor.Domain.DependencyInjection
{
    public static class OrderSupervisorServiceExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<ICounterService, CounterService>();
            services.AddScoped<IHandler<CreateOrderCommand>, Handlers.Order.OrderSaga>();
            return services;
        }
    }
}
