using OrderSolution.Core.Commands;
using OrderSolution.Infrastructure.Storage.Abstractions.Text;
using System.Threading;
using System.Threading.Tasks;

namespace OrderSolution.Infrastructure.Storage.Abstractions.Communication
{
    public interface IBus
    {
        Task<Message> TakeMessageAsync(CancellationToken cancellationToken);
        Task SendCommandAsync<T>(T command) where T : ICommand;
    }
}
