using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Services;
using Domain;
using Domain.Entities;
using FluentAssertions;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace Application.UnitTests.Services
{
    public class SupervisorServiceTests
    {
        private readonly ISupervisorService _sut;
        private readonly Mock<IApplicationDbContext> _context;
        private readonly Mock<IHttpService> _httpService;

        public SupervisorServiceTests()
        {
            _context = new Mock<IApplicationDbContext>();
            _httpService = new Mock<IHttpService>();
            _sut = new SupervisorService(_context.Object, _httpService.Object);
        }
        
        [Fact]
        public async Task GetLeastLoadedSupervisorId_Should_Return_Supervisor_Who_HasNoTransaction()
        {
            var userIds = Enumerable.Repeat(Guid.NewGuid(), 10).ToList();
            _httpService.Setup(x => x.GetUserIdsInRole(It.IsAny<string>()))
                .ReturnsAsync(userIds);

            var transactions = new List<Transaction>()
            {
                new() { AssignedSupervisorId = userIds[0] },
                new() { AssignedSupervisorId = userIds[0] },
                new() { AssignedSupervisorId = userIds[0] },
                new() { AssignedSupervisorId = userIds[0] },
                new() { AssignedSupervisorId = userIds[1] },
                new() { AssignedSupervisorId = userIds[1] },
                new() { AssignedSupervisorId = userIds[3] },
                new() { AssignedSupervisorId = userIds[4] },
                new() { AssignedSupervisorId = userIds[6] },
                new() { AssignedSupervisorId = userIds[6] }
            };
            var mockedTransactions = transactions.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Transactions)
                .Returns(mockedTransactions.Object);

            var result = await _sut.GetLeastLoadedSupervisorId(Roles.Supervisor);

            result.Should().Be(userIds[2]);
        }
        
        [Fact]
        public async Task GetLeastLoadedSupervisorId_Should_Return_Supervisor_Who_HasLeastTransactions()
        {
            var userIds = Enumerable.Repeat(Guid.NewGuid(), 10).ToList();
            _httpService.Setup(x => x.GetUserIdsInRole(It.IsAny<string>()))
                .ReturnsAsync(userIds);

            var transactions = new List<Transaction>()
            {
                new() { AssignedSupervisorId = userIds[0] },
                new() { AssignedSupervisorId = userIds[1] },
                new() { AssignedSupervisorId = userIds[1] },
                new() { AssignedSupervisorId = userIds[2] },
                new() { AssignedSupervisorId = userIds[2] },
                new() { AssignedSupervisorId = userIds[3] },
                new() { AssignedSupervisorId = userIds[3] },
                new() { AssignedSupervisorId = userIds[4] },
                new() { AssignedSupervisorId = userIds[4] },
                new() { AssignedSupervisorId = userIds[5] },
                new() { AssignedSupervisorId = userIds[5] },
                new() { AssignedSupervisorId = userIds[6] },
                new() { AssignedSupervisorId = userIds[6] },
                new() { AssignedSupervisorId = userIds[7] },
                new() { AssignedSupervisorId = userIds[7] },
                new() { AssignedSupervisorId = userIds[8] },
                new() { AssignedSupervisorId = userIds[8] },
                new() { AssignedSupervisorId = userIds[9] },
                new() { AssignedSupervisorId = userIds[9] }
            };
            var mockedTransactions = transactions.AsQueryable().BuildMockDbSet();
            _context.Setup(x => x.Transactions)
                .Returns(mockedTransactions.Object);

            var result = await _sut.GetLeastLoadedSupervisorId(Roles.Supervisor);

            result.Should().Be(userIds[0]);
        }
    }
}