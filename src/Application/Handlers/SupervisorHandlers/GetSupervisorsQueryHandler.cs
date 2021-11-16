using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models.ApiModels.Supervisors.Queries;
using Application.Models.DTOs;
using MediatR;

namespace Application.Handlers.SupervisorHandlers
{
    public class GetSupervisorsQueryHandler : IRequestHandler<GetSupervisorsQuery, PaginatedList<SupervisorObject>>
    {
        private readonly ISupervisorService _supervisorService;

        public GetSupervisorsQueryHandler(ISupervisorService supervisorService)
        {
            _supervisorService = supervisorService;
        }

        public async Task<PaginatedList<SupervisorObject>> Handle(GetSupervisorsQuery request,
            CancellationToken cancellationToken)
        {
            var supervisors = await _supervisorService.GetSupervisorsAsync(request.SearchQuery);
            return PaginatedList<SupervisorObject>.Create(supervisors, request.PageIndex, request.PageSize);
        }
    }
}