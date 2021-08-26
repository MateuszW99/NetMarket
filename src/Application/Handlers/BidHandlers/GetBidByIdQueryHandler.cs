using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Bids.Queries;
using Application.Models.DTOs;
using AutoMapper;
using MediatR;

namespace Application.Handlers.BidHandlers
{
    public class GetBidByIdQueryHandler : IRequestHandler<GetBidByIdQuery, BidObject>
    {
        private readonly IBidService _bidService;
        private readonly IHttpService _httpService;
        private readonly IMapper _mapper;

        public GetBidByIdQueryHandler(IBidService bidService, IHttpService httpService, IMapper mapper)
        {
            _bidService = bidService;
            _httpService = httpService;
            _mapper = mapper;
        }

        public async Task<BidObject> Handle(GetBidByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpService.GetUserId();
            var bid = await _bidService.GetBidByIdAsync(Guid.Parse(request.Id));

            if (bid == null)
            {
                throw new NotFoundException(nameof(bid), request.Id);
            }
            
            if (bid.CreatedBy != Guid.Parse(userId))
            {
                throw new UnauthorizedAccessException($"Authorization rules violated by user {userId}");
            }

            return _mapper.Map<BidObject>(bid);
        }
    }
}