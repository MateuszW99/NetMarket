using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Behaviours;
using Application.Common.Models;
using Application.Handlers.BidHandlers;
using Application.Models.ApiModels.Bids.Queries;
using Application.Models.DTOs;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace Application.UnitTests.Handlers.BidHandlers
{
    public class QueryHandlers
    {
        #region GetBidByIdQueryHandler

        [Fact]
        public async Task GetBidByIdQueryHandlerShouldNotThrowWhenPropertiesAreValid()
        {
            var getBidByIdQuery = new GetBidByIdQuery()
            {
                Id = Guid.NewGuid().ToString()
            };

            var queryHandler = new GetBidByIdQueryHandler(null, null, null);
            var validationBehaviour = new ValidationBehaviour<GetBidByIdQuery, BidObject>(new List<GetBidByIdQueryValidator>
            {
                new()
            }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(getBidByIdQuery,
                    CancellationToken.None,
                    () => queryHandler.Handle(getBidByIdQuery, CancellationToken.None)))
                .Should()
                .NotThrowAsync<ValidationException>();
        }
        
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("randomString")]
        [InlineData("18cc36cab7c54d38b11930d78ff3d433")]
        [InlineData("73c13a-f8-45ae-9be-d48a4aeb6b2c")]
        [InlineData("73c13a85 caf8 45ae 9bea d48a4aeb6b2c")]
        [InlineData("{73c13a85-caf8-45ae-9bea-d48a4aeb6b2c}")]
        public async Task GetBidByIdQueryHandlerShouldThrowWhenPropertiesAreInvalid(string id)
        {
            var getBidByIdQuery = new GetBidByIdQuery()
            {
                Id = id
            };

            var queryHandler = new GetBidByIdQueryHandler(null, null, null);
            var validationBehaviour = new ValidationBehaviour<GetBidByIdQuery, BidObject>(new List<GetBidByIdQueryValidator>
            {
                new()
            }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(getBidByIdQuery,
                    CancellationToken.None,
                    () => queryHandler.Handle(getBidByIdQuery, CancellationToken.None)))
                .Should()
                .ThrowAsync<ValidationException>();
        }
        
        #endregion

        #region GetUserBidsQueryHandler

        [Fact]
        public async Task GetUserBidsQueryHandlerShouldNotThrowWhenPropertiesAreNotInitialized()
        {
            var getUserBidsQuery = new GetUserBidsQuery()
            {
                PageSize = 10,
                PageIndex = 1
            };

            var queryHandler = new GetUserBidsQueryHandler(null, null, null);
            var validationBehaviour = new ValidationBehaviour<GetUserBidsQuery, PaginatedList<BidObject>>(new List<GetUserBidsQueryValidator>
            {
                new()
            }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(getUserBidsQuery,
                    CancellationToken.None,
                    () => queryHandler.Handle(getUserBidsQuery, CancellationToken.None)))
                .Should()
                .NotThrowAsync<ValidationException>();
        }
        
        [Fact]
        public async Task GetUserBidsQueryHandlerShouldNotThrowWhenPropertiesAreInitialized()
        {
            var getUserBidsQuery = new GetUserBidsQuery()
            {
                PageSize = 100,
                PageIndex = 2
            };

            var queryHandler = new GetUserBidsQueryHandler(null, null, null);
            var validationBehaviour = new ValidationBehaviour<GetUserBidsQuery, PaginatedList<BidObject>>(new List<GetUserBidsQueryValidator>
            {
                new()
            }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(getUserBidsQuery,
                    CancellationToken.None,
                    () => queryHandler.Handle(getUserBidsQuery, CancellationToken.None)))
                .Should()
                .NotThrowAsync<ValidationException>();
        }
        
        [Theory]
        [InlineData(0, 0)]
        [InlineData(10, 0)]
        [InlineData(0, -1)]
        [InlineData(-1, -1)]
        [InlineData(0, 10)]
        public async Task GetUserBidsQueryHandlerShouldThrowWhenPropertiesAreInvalid(int pageSize, int pageIndex)
        {
            var getUserBidsQuery = new GetUserBidsQuery()
            {
                PageSize = pageSize,
                PageIndex = pageIndex
            };

            var queryHandler = new GetUserBidsQueryHandler(null, null, null);
            var validationBehaviour = new ValidationBehaviour<GetUserBidsQuery, PaginatedList<BidObject>>(new List<GetUserBidsQueryValidator>
            {
                new()
            }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(getUserBidsQuery,
                    CancellationToken.None,
                    () => queryHandler.Handle(getUserBidsQuery, CancellationToken.None)))
                .Should()
                .ThrowAsync<ValidationException>();
        }

        #endregion
    }
}