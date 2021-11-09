using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class UserSeeder : ISeeder
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserSeeder(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            await SeedAdminUser();
            await SeedSupervisorUser();
        }

        private async Task SeedAdminUser()
        {
            const string email = "admin@gmail.com";
            var adminUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (adminUser != null)
            {
                return;
            }

            adminUser = new ApplicationUser()
            {
                UserName = "admin",
                Email = email
            };

            await _userManager.CreateAsync(adminUser, "Admin123!");
            await _userManager.AddToRoleAsync(adminUser, Roles.User);
            await _userManager.AddToRoleAsync(adminUser, Roles.Supervisor);
            await _userManager.AddToRoleAsync(adminUser, Roles.Admin);
        }

        private async Task SeedSupervisorUser()
        {
            const string email = "supervisor@gmail.com";

            var supervisorUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);

            if (supervisorUser != null)
            {
                return;
            }

            supervisorUser = new ApplicationUser()
            {
                UserName = "supervisor",
                Email = email
            };

            await _userManager.CreateAsync(supervisorUser, "Supervisor123!");
            await _userManager.AddToRoleAsync(supervisorUser, Roles.User);
            await _userManager.AddToRoleAsync(supervisorUser, Roles.Supervisor);
        }
    }
}