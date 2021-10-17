using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models.ApiModels.Bids.Queries;
using Application.Models.DTOs;
using AutoMapper;
using MediatR;

namespace Application.Handlers.BidHandlers
{
    public class GetUserBidsQueryHandler : IRequestHandler<GetUserBidsQuery, PaginatedList<BidObject>>
    {
        private readonly IBidService _bidService;
        private readonly IMapper _mapper;
        private readonly IHttpService _httpService;

        public GetUserBidsQueryHandler(IBidService bidService, IMapper mapper, IHttpService httpService)
        {
            _bidService = bidService;
            _mapper = mapper;
            _httpService = httpService;
        }

        public async Task<PaginatedList<BidObject>> Handle(GetUserBidsQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpService.GetUserId();
            var bids = await _bidService.GetUserBids(Guid.Parse(userId));
            var mappedBids = _mapper.Map<List<BidObject>>(bids);
            return PaginatedList<BidObject>.Create(mappedBids, request.PageIndex, request.PageSize);
        }
    }
}