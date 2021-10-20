using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models.ApiModels.Asks.Queries;
using Application.Models.DTOs;
using AutoMapper;
using MediatR;

namespace Application.Handlers.AskHandlers
{
    public class GetUserAsksQueryHandler : IRequestHandler<GetUserAsksQuery, PaginatedList<AskObject>>
    {
        private readonly IAskService _askService;
        private readonly IMapper _mapper;
        private readonly IHttpService _httpService;

        public GetUserAsksQueryHandler(IAskService askService, IMapper mapper, IHttpService httpService)
        {
            _askService = askService;
            _mapper = mapper;
            _httpService = httpService;
        }

        public async Task<PaginatedList<AskObject>> Handle(GetUserAsksQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpService.GetUserId();
            var asks = await _askService.GetUserAsks(Guid.Parse(userId));
            var mappedAsks = _mapper.Map<List<AskObject>>(asks);
            return PaginatedList<AskObject>.Create(mappedAsks, request.PageIndex, request.PageSize);
        }
    }
}