using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Services;
using Application.Common.Interfaces;
using Domain;
using FluentAssertions;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace Application.UnitTests.Services
{
    public class UserManagerServiceTests
    {
        private readonly Mock<IUserStore<ApplicationUser>> _userStore;
        private readonly Mock<UserManager<ApplicationUser>> _userManager;
        private readonly IUserManagerService _sut;

        public UserManagerServiceTests()
        {
            _userStore = new Mock<IUserStore<ApplicationUser>>();
            _userManager = new Mock<UserManager<ApplicationUser>>(_userStore.Object, null, null, null, null, null, null, null, null);
            _sut = new UserManagerService(_userManager.Object);
        }
        
        [Fact]
        public async Task GetUserIdsInRole_Should_Return_ListOfUsers()
        {
            var users = new List<ApplicationUser> { new() };
            _userManager.Setup(x => x.GetUsersInRoleAsync(It.IsAny<string>()))
                .ReturnsAsync(users);

            var result = await _sut.GetUserIdsInRole(Roles.Supervisor);

            result.Should().NotBeNull();
            result.Count.Should().Be(users.Count);
        }
    }
}