using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Models.ApiModels.Transactions.Queries;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using FluentAssertions;
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
        public async Task GetTransactionsByStatus_Should_ReturnList_of_ProperTransactions()
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
        public async Task GetTransactionsByStatus_Should_ReturnEmptyList_of_Transactions_when_TheyDontExist()
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
        public async Task GetSupervisorTransactions_Should_ReturnList_of_SupervisorsTransactions()
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
    }
    
    
}