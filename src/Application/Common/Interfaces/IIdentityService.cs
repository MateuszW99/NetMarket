using System.Threading.Tasks;
using Application.Identity;
using Application.Identity.Requests;

namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string username, string password);
        Task<AuthenticationResult> LoginAsync(string email, string password);
    }
}