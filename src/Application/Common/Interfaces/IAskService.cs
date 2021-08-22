using System;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Models.DTOs;

namespace Application.Common.Interfaces
{
    public interface IAskService
    {
        Task<AskObject> GetAskById(Guid userId, Guid askId);
        Task<PaginatedList<AskObject>> GetUserAsks(Guid userId);
        Task CreateAsk(Guid userId);
        Task UpdateAsk(Guid userId);
        Task DeleteAsk(Guid userId);
    }
}