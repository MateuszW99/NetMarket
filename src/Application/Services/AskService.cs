using System;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models.DTOs;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class AskService : IAskService
    {
        private readonly IApplicationDbContext _context;
        private readonly Logger<AskService> _logger;
        private readonly IMapper _mapper;

        public AskService(IApplicationDbContext context, Logger<AskService> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public Task<AskObject> GetAskById(Guid userId, Guid askId)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedList<AskObject>> GetUserAsks(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsk(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsk(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsk(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}