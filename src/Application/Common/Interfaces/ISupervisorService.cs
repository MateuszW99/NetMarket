using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Models.DTOs;

namespace Application.Common.Interfaces
{
    public interface ISupervisorService
    {
        Task<Guid> GetLeastLoadedSupervisorId(string role);
        Task<List<SupervisorObject>> GetSupervisorsAsync(string searchQuery);
    }
}