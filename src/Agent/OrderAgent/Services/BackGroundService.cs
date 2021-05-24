using OrderSolution.Infrastructure.Storage.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrderAgent.Domain;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderAgent.Services
{
    class BackGroundService : BackgroundService
    {
        private readonly IStorageEnvironmentConfiguration storageEnvironmentConfiguration;
        private readonly IAgent agent;
        private readonly ILogger<BackgroundService> logger;

        public BackGroundService(IStorageEnvironmentConfiguration storageEnvironmentConfiguration, IAgent agent, ILogger<BackgroundService> logger)
        {
            this.storageEnvironmentConfiguration = storageEnvironmentConfiguration;
            this.agent = agent;
            this.logger = logger;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            storageEnvironmentConfiguration.Initialize();
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {            
            await agent.Execute(stoppingToken);
            Console.WriteLine("Enter with a key to finish this proccess");
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}
