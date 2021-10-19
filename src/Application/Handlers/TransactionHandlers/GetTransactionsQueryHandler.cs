using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models.ApiModels.Transactions.Queries;
using Application.Models.DTOs;
using AutoMapper;
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
            var rawTransactions = await _transactionService.GetTransactionsByStatus(request.SearchTransactionsQuery);
            var transactions = _mapper.Map<List<TransactionObject>>(rawTransactions);
            return PaginatedList<TransactionObject>.Create(transactions,
                request.SearchTransactionsQuery.PageIndex, request.SearchTransactionsQuery.PageSize);
        }
    }
}