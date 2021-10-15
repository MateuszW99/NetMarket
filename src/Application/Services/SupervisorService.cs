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
        private readonly IHttpService _httpService;

        public SupervisorService(IApplicationDbContext context, IHttpService httpService)
        {
            _context = context;
            _httpService = httpService;
        }
        
        public async Task<Guid> GetLeastLoadedSupervisorId(string role)
        {
            var supervisorIds = await _httpService.GetUserIdsInRole(role);
            var transactions = await _context.Transactions
                .Where(x => 
                    supervisorIds.Contains(x.AssignedSupervisorId))
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