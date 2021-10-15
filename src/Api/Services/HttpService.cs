using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Api.Services
{
    public class HttpService : IHttpService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public HttpService(IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public string GetUserId()
        {
            if (_httpContextAccessor.HttpContext?.User == null)
            {
                return String.Empty;
            }

            return _httpContextAccessor.HttpContext.User.FindFirstValue("id") ?? string.Empty;
        }

        public async Task<List<Guid>> GetUserIdsInRole(string role)
        {
            var supervisors = await _userManager.GetUsersInRoleAsync(role);
            return supervisors.Select(x => x.Id).ToList();
        }
    }
}