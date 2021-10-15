using System;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface ISupervisorService
    {
        Task<Guid> GetLeastLoadedSupervisorId();
    }
}