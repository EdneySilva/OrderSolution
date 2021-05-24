
using System.Threading.Tasks;

namespace OrderSupervisor.Domain.Services
{
    public interface ICounterService
    {
        Task<int> NextValueAsync();
    }
}
