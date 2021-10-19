using System;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests.Helpers
{
    public static class AuthHelper
    {
        public static async Task<string> RunAsFirstUserAsync(CustomWebApplicationFactory factory)
        {
            return await RunAsUserAsync(factory, FirstUser.Email, FirstUser.Password, Roles.User);
        }

        public static async Task<string> RunAsOtherUserAsync(CustomWebApplicationFactory factory)
        {
            return await RunAsUserAsync(factory, OtherUser.Email, OtherUser.Password, Roles.User);
        }
        
        public static async Task<string> RunAsSupervisorAsync(CustomWebApplicationFactory factory)
        {
            return await RunAsUserAsync(factory, SupervisorUser.Email, SupervisorUser.Password, Roles.Supervisor);
        }
        
        public static async Task<string> RunAsAdministratorAsync(CustomWebApplicationFactory factory)
        {
            return await RunAsUserAsync(factory, AdminUser.Email, AdminUser.Password, Roles.Admin);
        }
        
        private static async Task<string> RunAsUserAsync(CustomWebApplicationFactory factory, string userName,
            string password, string role)
        {
            using var scope = factory.Services.CreateScope();
            var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole<Guid>>>();

            if (userManager == null || roleManager == null)
            {
                throw new Exception("Failed to get user or role manager");
            }

            var user = new ApplicationUser { Email = userName, UserName = userName };

            var createdUser = await userManager.CreateAsync(user, password);

            if (createdUser.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>()
                    {
                        Name = role,
                        NormalizedName = role.ToUpper()
                    });
                }

                await userManager.AddToRoleAsync(user, role);
                factory.CurrentUserId = user.Id.ToString();

                return factory.CurrentUserId;
            }

            var errors = string.Join(Environment.NewLine, createdUser.Errors);

            throw new Exception($"Unable to create {userName}.{Environment.NewLine}{errors}");
        }
    }
}