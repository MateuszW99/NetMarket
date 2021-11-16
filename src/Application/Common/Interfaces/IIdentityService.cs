using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Identity;
using Application.Identity.Requests;
using Application.Identity.Responses;
using Application.Models.DTOs;

namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsync(string email, string username, string password, string role = "User");
        Task<AuthenticationResult> LoginAsync(string email, string password);
        Task<ResetPasswordResponse> ResetPassword(string email, string password, string newPassword);
        Task<List<SupervisorObject>> GetSupervisorsAsync();
        Task<DeleteUserResponse> DeleteUserAsync(string id);
    }
}