using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Models.ApiModels.Transactions.Queries;
using Application.Models.DTOs;
using AutoMapper;
using MediatR;

namespace Application.Handlers.TransactionHandlers
{
    public class GetTransactionByIdQueryHandler : IRequestHandler<GetTransactionByIdQuery, TransactionObject>
    {
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IHttpService _httpService;

        public GetTransactionByIdQueryHandler(IMapper mapper, ITransactionService transactionService, IHttpService httpService)
        {
            _mapper = mapper;
            _transactionService = transactionService;
            _httpService = httpService;
        }
        
        public async Task<TransactionObject> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
        {
            var supervisorId = _httpService.GetUserId();
            var transaction = await _transactionService.GetTransactionByIdAsync(request.Id, supervisorId);

            return _mapper.Map<TransactionObject>(transaction);
        }
    }
}