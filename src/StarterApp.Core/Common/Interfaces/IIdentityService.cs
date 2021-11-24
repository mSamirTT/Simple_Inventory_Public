using StarterApp.Core.Common.Models;
using System.Threading.Tasks;

namespace StarterApp.Core.Common.Interfaces
{
    public interface IIdentityService
    {
        public string UserId { get; }
        public string UserName { get; }

        Task<(Result Result, string UserId)> CreateUserAsync(string userName, string password);
        
        Task<Result> DeleteUserAsync(string userId);
    }
}
