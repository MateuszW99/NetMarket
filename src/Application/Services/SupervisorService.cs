using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class SupervisorService : ISupervisorService
    {
        private readonly IApplicationDbContext _context;
        private readonly IUserManagerService _userManagerService;

        public SupervisorService(IApplicationDbContext context, IUserManagerService userManagerService)
        {
            _context = context;
            _userManagerService = userManagerService;
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
    }
}