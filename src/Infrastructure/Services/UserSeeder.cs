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
            var email = "admin@gmail.com";
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
        
    }
}