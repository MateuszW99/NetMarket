using System.Threading.Tasks;
using Application.Identity;
using Application.Identity.Requests;
using Application.Identity.Responses;

namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string username, string password);
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<ResetPasswordResponse> ResetPassword(string email, string password, string newPassword);
    }
}