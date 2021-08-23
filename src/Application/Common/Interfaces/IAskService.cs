using System;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Models.ApiModels.Asks.Commands;
using Application.Models.DTOs;

namespace Application.Common.Interfaces
{
    public interface IAskService
    {
        Task<AskObject> GetAskById(Guid userId, Guid askId);
        Task<PaginatedList<AskObject>> GetUserAsks(Guid userId);
        Task CreateAsk(CreateAskCommand command, Guid userId);
        Task UpdateAsk(UpdateAskCommand command, Guid userId);
        Task DeleteAsk(Guid askId, Guid userId);
    }
}