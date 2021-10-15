using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Services;
using Application.Common.Interfaces;
using Domain;
using FluentAssertions;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Moq;
using Xunit;

namespace Application.UnitTests.Services
{
    public class HttpServiceTests
    {
        private readonly IHttpService _sut;
        private readonly Mock<IUserStore<ApplicationUser>> _userStore;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly Mock<UserManager<ApplicationUser>> _userManager;

        public HttpServiceTests()
        {
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _userStore = new Mock<IUserStore<ApplicationUser>>();
            _userManager = new Mock<UserManager<ApplicationUser>>(_userStore.Object, null, null, null, null, null, null, null, null);
            _sut = new HttpService(_httpContextAccessor.Object, _userManager.Object);
        }

        [Fact]
        public async Task GetUserId_Should_Return_UserId_When_UserIsSignedIn()
        {
            var userId = Guid.NewGuid();
            var context = new DefaultHttpContext();
            context.User.AddIdentity(new ClaimsIdentity(new []{ new Claim("id", userId.ToString()) }));
            _httpContextAccessor.Setup(x => x.HttpContext).Returns(context);

            var result = _sut.GetUserId();

            result.Should().NotBeNullOrEmpty();
            result.Should().Be(userId.ToString());
        }
        
        [Fact]
        public async Task GetUserId_Should_Return_UserId_When_UserIsNotSignedIn()
        {
            var userId = Guid.NewGuid();
            var context = new DefaultHttpContext();
            _httpContextAccessor.Setup(x => x.HttpContext).Returns(context);

            var result = _sut.GetUserId();

            result.Should().BeNullOrEmpty();
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