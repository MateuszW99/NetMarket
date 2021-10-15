using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Mappings;
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
            var queriedBids = _bidService.GetUserBids(Guid.Parse(userId));
            var mappedBids = await queriedBids.ProjectToListAsync<BidObject>(_mapper.ConfigurationProvider);
            return PaginatedList<BidObject>.Create(mappedBids, request.PageIndex, request.PageSize);
        }
    }
}