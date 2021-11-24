using System.Threading;
using System.Threading.Tasks;

namespace StarterApp.Core.Common.Interfaces
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
