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
    public class GetTransactionsQueryHandler : IRequestHandler<GetTransactionsQuery, PaginatedList<TransactionObject>>
    {
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;

        public GetTransactionsQueryHandler(IMapper mapper, ITransactionService transactionService)
        {
            _mapper = mapper;
            _transactionService = transactionService;
        }

        public async Task<PaginatedList<TransactionObject>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            var queredTransactions = _transactionService.GetTransactions(request.SearchTransactionsQuery);
            
            return await queredTransactions.ProjectTo<TransactionObject>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.SearchTransactionsQuery.PageIndex, request.SearchTransactionsQuery.PageSize);
        }
    }
}