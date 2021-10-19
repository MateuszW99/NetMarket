using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models.ApiModels.Transactions.Commands;
using Application.Models.ApiModels.Transactions.Queries;
using Application.Models.DTOs;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace Application.UnitTests.Services
{
    public class TransactionServiceTests
    {
        private readonly ITransactionService _sut;
        private readonly Mock<IApplicationDbContext> _context;

        public TransactionServiceTests()
        {
            _context = new Mock<IApplicationDbContext>();
            _sut = new TransactionService(_context.Object);
        }

        [Fact]
        public async Task GetTransactionsByStatus_Should_ReturnListOfTransactions()
        {
            var transactions = new List<Transaction>()
            {
                new() { Status = TransactionStatus.Arrived },
                new() { Status = TransactionStatus.Arrived },
                new() { Status = TransactionStatus.Arrived },
                new() { Status = TransactionStatus.EnRouteFromSeller },
                new() { Status = TransactionStatus.Started }
            };

            var mockedTransactions = transactions.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Transactions).Returns(mockedTransactions.Object);

            var result = await _sut.GetTransactionsByStatus(new SearchTransactionsQuery()
            {
                Status = TransactionStatus.Started.ToString()
            });

            result.Should().NotBeNull();
            result.Count.Should().Be(transactions.Count(x => x.Status == TransactionStatus.Started));
        }

        [Fact]
        public async Task GetTransactionsByStatus_Should_ReturnEmptyListOfTransactions_when_TheyDontExist()
        {
            var transactions = new List<Transaction>()
            {
                new() { Status = TransactionStatus.Arrived },
                new() { Status = TransactionStatus.Arrived },
                new() { Status = TransactionStatus.Arrived },
                new() { Status = TransactionStatus.EnRouteFromSeller },
                new() { Status = TransactionStatus.Started }
            };

            var mockedTransactions = transactions.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Transactions).Returns(mockedTransactions.Object);

            var result = await _sut.GetTransactionsByStatus(new SearchTransactionsQuery()
            {
                Status = TransactionStatus.Checked.ToString()
            });

            result.Should().NotBeNull();
            result.Count.Should().Be(transactions.Count(x => x.Status == TransactionStatus.Checked));
        }

        [Fact]
        public async Task GetSupervisorTransactions_Should_Return_ListOfTransactions()
        {
            var supervisorId = Guid.NewGuid();
            var transactions = new List<Transaction>()
            {
                new() { Status = TransactionStatus.EnRouteFromSeller, AssignedSupervisorId = supervisorId },
                new() { Status = TransactionStatus.Arrived },
                new() { Status = TransactionStatus.Arrived },
                new() { Status = TransactionStatus.Arrived, AssignedSupervisorId = supervisorId },
                new() { Status = TransactionStatus.Started }
            };
            
            var mockedTransactions = transactions.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Transactions).Returns(mockedTransactions.Object);
            
            var result = await _sut.GetSupervisorTransactions(new SearchTransactionsQuery()
                {
                    Status = TransactionStatus.Arrived.ToString()
                },
                supervisorId.ToString());

            result.Should().NotBeNull();
            result.Count.Should().Be(transactions.Count(x => x.AssignedSupervisorId == supervisorId && x.Status == TransactionStatus.Arrived));
        }
        
        [Fact]
        public async Task GetTransactionByIdAsync_Should_Return_Transaction()
        {
            var id = Guid.NewGuid();
            var supervisorId = Guid.NewGuid();
            var transactions = new List<Transaction>()
            {
                new() { Id = Guid.NewGuid(), AssignedSupervisorId = supervisorId},
                new() { Id = id, AssignedSupervisorId = supervisorId},
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };

            var mockedTransactions = transactions.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Transactions).Returns(mockedTransactions.Object);

            var result = await _sut.GetTransactionByIdAsync(id.ToString(), supervisorId.ToString());

            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.AssignedSupervisorId.Should().Be(supervisorId);
        }
        
        [Fact]
        public async Task GetTransactionByIdAsync_Should_Throw_NotFoundException_When_QueryingNotExistingTransaction()
        {
            var id = Guid.NewGuid();
            var supervisorId = Guid.NewGuid();
            var transactions = new List<Transaction>()
            {
                new() { Id = Guid.NewGuid(), AssignedSupervisorId = supervisorId},
                new() { Id = id, AssignedSupervisorId = supervisorId},
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };

            var mockedTransactions = transactions.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Transactions).Returns(mockedTransactions.Object);
            
            await FluentActions.Awaiting(() => _sut.GetTransactionByIdAsync(Guid.NewGuid().ToString(), supervisorId.ToString()))
                .Should()
                .ThrowAsync<NotFoundException>();
        }
        
        [Fact]
        public async Task GetTransactionByIdAsync_Should_Throw_ForbiddenAccessException_When_QueryingTransactionWithWrongSupervisorId()
        {
            var id = Guid.NewGuid();
            var supervisorId = Guid.NewGuid();
            var transactions = new List<Transaction>()
            {
                new() { Id = id, AssignedSupervisorId = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };

            var mockedTransactions = transactions.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Transactions).Returns(mockedTransactions.Object);
            
            await FluentActions.Awaiting(() => _sut.GetTransactionByIdAsync(id.ToString(), supervisorId.ToString()))
                .Should()
                .ThrowAsync<ForbiddenAccessException>();
        }

        [Fact]
        public async Task UpdateTransactionStatusAsync_Should_Update()
        {
            var id = Guid.NewGuid();
            var supervisorId = Guid.NewGuid();

            var transactionBefore = new Transaction()
            {
                Id = id,
                Status = TransactionStatus.Started,
                AssignedSupervisorId = supervisorId
            };
            
            var transactions = new List<Transaction>()
            {
                transactionBefore,
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };
            var transactionsBefore = transactions.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Transactions).Returns(transactionsBefore.Object);
            
            var transactionAfter = new Transaction()
            {
                Id = id,
                Status = TransactionStatus.Arrived,
                AssignedSupervisorId = supervisorId
            };
            transactions = new List<Transaction>()
            {
                transactionAfter,
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };
            var transactionsAfter = transactions.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Transactions.Update(It.IsAny<Transaction>()))
                .Callback(() => _context.Setup(x => x.Transactions).Returns(transactionsAfter.Object));

            await _sut.UpdateTransactionStatusAsync(
                new UpdateTransactionStatusCommand { Id = id.ToString(), Status = TransactionStatus.Arrived.ToString() }, 
                supervisorId.ToString(),
                CancellationToken.None);

            var result = await _context.Object.Transactions.FirstOrDefaultAsync(x => x.Id == id);
            result.Should().NotBeNull();
            result.Id.Should().Be(id);
            result.AssignedSupervisorId.Should().Be(supervisorId);
            result.Status.Should().Be(TransactionStatus.Arrived);
        }
        
        [Fact]
        public async Task UpdateTransactionStatusAsync_Should_Throw_NotFoundException_When_TransactionDoesntExist()
        {
            var id = Guid.NewGuid();
            var supervisorId = Guid.NewGuid();

            var transactionBefore = new Transaction()
            {
                Id = Guid.NewGuid(),
                Status = TransactionStatus.Started,
                AssignedSupervisorId = supervisorId
            };
            
            var transactions = new List<Transaction>()
            {
                transactionBefore,
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };
            var transactionsBefore = transactions.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Transactions).Returns(transactionsBefore.Object);

            await FluentActions.Awaiting(() => _sut.UpdateTransactionStatusAsync(
                    new UpdateTransactionStatusCommand
                        { Id = id.ToString(), Status = TransactionStatus.Arrived.ToString() },
                    supervisorId.ToString(),
                    CancellationToken.None))
                .Should()
                .ThrowAsync<NotFoundException>();
        }
        
        [Fact]
        public async Task UpdateTransactionStatusAsync_Should_Throw_ForbiddenAccessException_When_QueryingTransactionWithWrongSupervisorId()
        {
            var id = Guid.NewGuid();
            var supervisorId = Guid.NewGuid();
            var transactions = new List<Transaction>()
            {
                new() { Id = id, AssignedSupervisorId = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() },
                new() { Id = Guid.NewGuid() }
            };
            var mockedTransactions = transactions.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Transactions).Returns(mockedTransactions.Object);
            
            await FluentActions.Awaiting(() => _sut.GetTransactionByIdAsync(id.ToString(), supervisorId.ToString()))
                .Should()
                .ThrowAsync<ForbiddenAccessException>();
        }

        [Fact]
        public async Task InitializeTransaction_Should_Create_Transaction()
        {
            var supervisorId = Guid.NewGuid();
            var ask = new Ask() { Id = Guid.NewGuid(), Price = 150M, SellerFee = 10M };
            var bid = new Bid() { Id = Guid.NewGuid(), Price = 150M, BuyerFee = 10M };
            var companyProfit = ask.SellerFee + bid.BuyerFee;
            var startDate = DateTime.Now;

            var transactions = new List<Transaction>();
            
            var transaction = new Transaction()
            {
                AssignedSupervisorId = supervisorId,
                CompanyProfit = ask.SellerFee + bid.BuyerFee,
                Ask = ask,
                AskId = ask.Id,
                SellerFee = ask.SellerFee,
                SellerPayout = ask.Price - ask.SellerFee,
                Bid = bid,
                BidId = bid.Id,
                TotalBuyerCost = bid.Price + bid.BuyerFee,
                BuyerFee = bid.BuyerFee,
                StartDate = startDate,
                Status = TransactionStatus.Started
            };
            
            var transactionsBefore = transactions.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Transactions).Returns(transactionsBefore.Object);
            
            transactions.Add(transaction);
            var transactionsAfter = transactions.AsQueryable().BuildMockDbSet();

            _context.Setup(x => x.Transactions.AddAsync(It.IsAny<Transaction>(), CancellationToken.None))
                .Callback(() => _context.Setup(x => x.Transactions).Returns(transactionsAfter.Object));

            await _sut.InitializeTransaction(ask, bid, startDate, supervisorId, CancellationToken.None);

            var newTransaction = await _context.Object.Transactions.FirstOrDefaultAsync();

            newTransaction.Should().NotBeNull();
            newTransaction.AskId.Should().Be(ask.Id);
            newTransaction.BidId.Should().Be(bid.Id);
            newTransaction.AssignedSupervisorId.Should().Be(supervisorId);
            newTransaction.StartDate.Should().Be(startDate);
            newTransaction.CompanyProfit.Should().Be(companyProfit);
            newTransaction.SellerFee.Should().Be(ask.SellerFee);
            newTransaction.SellerPayout.Should().Be(ask.Price - ask.SellerFee);
            newTransaction.TotalBuyerCost.Should().Be(bid.Price + bid.BuyerFee);
            newTransaction.Status.Should().Be(TransactionStatus.Started);
        }
    }
    
}

