using System;
using System.Threading;
using System.Threading.Tasks;

namespace OrderAgent.Domain
{
    public interface IAgent
    {
        Guid AgentId { get; }
        int MagicNumber { get; }
        Task Execute(CancellationToken stoppingToken);
    }
}
