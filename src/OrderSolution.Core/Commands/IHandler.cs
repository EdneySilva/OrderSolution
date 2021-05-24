using System.Threading.Tasks;

namespace OrderSolution.Core.Commands
{
    public interface IHandler<T> where T : ICommand
    {
        Task<ICommandResult> Handle(T command);
    }
}
