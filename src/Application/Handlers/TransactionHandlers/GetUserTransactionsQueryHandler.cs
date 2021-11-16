using System;
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
    public class
        GetUserTransactionsQueryHandler : IRequestHandler<GetUserTransactionsQuery, PaginatedList<TransactionObject>>
    {
        private readonly IMapper _mapper;
        private readonly ITransactionService _transactionService;
        private readonly IHttpService _httpService;

        public GetUserTransactionsQueryHandler(IMapper mapper, ITransactionService transactionService,
            IHttpService httpService)
        {
            _mapper = mapper;
            _transactionService = transactionService;
            _httpService = httpService;
        }

        public async Task<PaginatedList<TransactionObject>> Handle(GetUserTransactionsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = _httpService.GetUserId();
            var rawTransactions =
                await _transactionService.GetUserTransactionsAsync(Guid.Parse(userId), request.SearchQuery);
            var transactions = _mapper.Map<List<TransactionObject>>(rawTransactions);
            return PaginatedList<TransactionObject>.Create(transactions,
                request.PageIndex, request.PageSize);
        }
    }
}