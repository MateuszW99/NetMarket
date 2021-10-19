using System;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services
{
    public class RoleSeeder : ISeeder
    {
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public RoleSeeder(RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            var roles = new[] { Roles.User, Roles.Supervisor, Roles.Admin };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid>()
                    {
                        Name = role,
                        NormalizedName = role.ToUpper()
                    });
                }
            }
        }
    }
}