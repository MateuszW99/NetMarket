using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Asks.Queries;
using Application.Models.DTOs;
using AutoMapper;
using MediatR;

namespace Application.Handlers.AskHandlers
{
    public class GetAskByIdQueryHandler : IRequestHandler<GetAskByIdQuery, AskObject>
    {
        private readonly IAskService _askService;
        private readonly IHttpService _httpService;
        private readonly IMapper _mapper;
        
        public GetAskByIdQueryHandler(IAskService askService, IHttpService httpService, IMapper mapper)
        {
            _askService = askService;
            _httpService = httpService;
            _mapper = mapper;
        }

        public async Task<AskObject> Handle(GetAskByIdQuery request, CancellationToken cancellationToken)
        {
            var userId = _httpService.GetUserId();
            var ask = await _askService.GetAskByIdAsync(Guid.Parse(request.Id));

            if (ask == null)
            {
                throw new NotFoundException(nameof(ask), request.Id);
            }
            
            if (ask.CreatedBy != Guid.Parse(userId))
            {
                throw new UnauthorizedAccessException($"Authorization rules violated by user {userId}");
            }

            return _mapper.Map<AskObject>(ask);
        }
    }
}