using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using Application.Models.ApiModels.Transactions.Queries;
using Application.Models.DTOs;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Handlers.TransactionHandlers
{
    public class GetSupervisorTransactionsQueryHandler : IRequestHandler<GetSupervisorTransactionsQuery, PaginatedList<TransactionObject>>
    {
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IHttpService _httpService;

        public GetSupervisorTransactionsQueryHandler(IMapper mapper, ITransactionService transactionService, IHttpService httpService)
        {
            _mapper = mapper;
            _transactionService = transactionService;
            _httpService = httpService;
        }
        
        public async Task<PaginatedList<TransactionObject>> Handle(GetSupervisorTransactionsQuery request, CancellationToken cancellationToken)
        {
            var supervisorId = _httpService.GetUserId();
            var queredTransactions = _transactionService.GetSupervisorTransactions(request.SearchTransactionsQuery, supervisorId);
            
            return await queredTransactions.ProjectTo<TransactionObject>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.SearchTransactionsQuery.PageIndex, request.SearchTransactionsQuery.PageSize);
        }
    }
}