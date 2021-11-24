using StarterApp.Infrastructure.Persistence;
using System.Threading.Tasks;

namespace StarterApp.Infrastructure.Common
{
    public interface IDomainEventsDispatcher
    {
        Task DispatchEventsAsync(ApplicationDbContext dbContext);
    }
}