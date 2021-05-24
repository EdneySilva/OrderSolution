using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace OrderSupervisor.Domain.Services
{
    public class CounterService : ICounterService
    {
        private static object state = new object();
        private readonly ILogger<CounterService> logger;
        private static int counter = 0;

        static CounterService()
        {
            var currentCounter = System.Environment.GetEnvironmentVariable("$OrderSupervisor");
            int.TryParse(currentCounter, out int localCount);
            counter = localCount;
        }

        public CounterService(ILogger<CounterService> logger)
        {
            this.logger = logger;
        }

        public Task<int> NextValueAsync()
        {
            Interlocked.Increment(ref counter);
            var value = counter;
            return Task.Run(() =>
            {
                lock(state)
                {
                    System.Environment.SetEnvironmentVariable("$OrderSupervisor", counter.ToString());
                    logger.LogDebug(System.Environment.GetEnvironmentVariable("$OrderSupervisor"));
                }
                return value;
            });
        }
    }
}
