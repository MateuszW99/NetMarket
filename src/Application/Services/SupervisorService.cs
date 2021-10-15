using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class SupervisorService : ISupervisorService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationDbContext _context;

        public SupervisorService(UserManager<ApplicationUser> userManager, IApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }


        public async Task<Guid> GetLeastLoadedSupervisorId()
        {
            var supervisors = await _userManager.GetUsersInRoleAsync(Roles.Supervisor);
            var transactions = await _context.Transactions
                .Where(x => 
                    supervisors.Select(y => y.Id)
                        .Contains(x.AssignedSupervisorId))
                .GroupBy(x => (x.AssignedSupervisorId))
                .Select(x => new
                    {
                        Id = x.Key, 
                        Count = x.Count()
                    })
                .OrderBy(x => x.Count)
                .ToListAsync();

            var supervisorsWithoutTasks = supervisors.Where(x => 
                    !transactions.Select(y => y.Id).Contains(x.Id))
                .ToList();
            
            return supervisorsWithoutTasks.Any() ? supervisorsWithoutTasks.First().Id : transactions.First().Id;
        }
    }
}