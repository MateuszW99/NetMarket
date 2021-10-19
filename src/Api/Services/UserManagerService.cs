using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Api.Services
{
    public class UserManagerService : IUserManagerService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserManagerService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        
        public async Task<List<Guid>> GetUserIdsInRole(string role)
        {
            var supervisors = await _userManager.GetUsersInRoleAsync(role);
            return supervisors.Select(x => x.Id).ToList();
        }
    }
}