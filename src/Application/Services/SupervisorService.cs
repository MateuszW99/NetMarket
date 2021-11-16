using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class SupervisorService : ISupervisorService
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserManagerService _userManagerService;
        private readonly IIdentityService _identityService;

        public SupervisorService(IApplicationDbContext context, IUserManagerService userManagerService,
            IIdentityService identityService)
        {
            _context = context;
            _userManagerService = userManagerService;
            _identityService = identityService;
        }

        public async Task<Guid> GetLeastLoadedSupervisorId(string role)
        {
            var supervisorIds = await _userManagerService.GetUserIdsInRole(role);
            var transactions = await _context.Transactions
                .GroupBy(x => (x.AssignedSupervisorId))
                .Select(x => new
                {
                    Id = x.Key,
                    Count = x.Count()
                })
                .OrderBy(x => x.Count)
                .ToListAsync();

            var supervisorsWithoutTasks = supervisorIds.Where(x =>
                    !transactions.Select(y => y.Id).Contains(x))
                .ToList();

            return supervisorsWithoutTasks.Any() ? supervisorsWithoutTasks.First() : transactions.First().Id;
        }

        public async Task<List<SupervisorObject>> GetSupervisorsAsync(string searchQuery)
        {
            var supervisors = await _identityService.GetSupervisorsAsync();
            var queriedSupervisors = supervisors.AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                queriedSupervisors = queriedSupervisors.Where(x =>
                    x.Email.ToLower()
                        .Contains(searchQuery.ToLower()) || x.Username.ToLower().Contains(searchQuery.ToLower()));
            }

            return queriedSupervisors.ToList();
        }
    }
}