using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Behaviours;
using Application.Common.Models;
using Application.Handlers.AskHandlers;
using Application.Models.ApiModels.Asks.Queries;
using Application.Models.DTOs;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace Application.UnitTests.Handlers.AskHandlers
{
    public class QueryHandlers
    {
        #region GetAskByIdQueryHandler

        [Fact]
        public async Task GetAskByIdQueryHandlerShouldNotThrowWhenPropertiesAreValid()
        {
            var getAskByIdQuery = new GetAskByIdQuery()
            {
                Id = Guid.NewGuid().ToString()
            };

            var queryHandler = new GetAskByIdQueryHandler(null, null, null);
            var validationBehaviour = new ValidationBehaviour<GetAskByIdQuery, AskObject>(new List<GetAskByIdQueryValidator>
            {
                new()
            }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(getAskByIdQuery,
                    CancellationToken.None,
                    () => queryHandler.Handle(getAskByIdQuery, CancellationToken.None)))
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
        public async Task GetAskByIdQueryHandlerShouldThrowWhenPropertiesAreInvalid(string id)
        {
            var getAskByIdQuery = new GetAskByIdQuery()
            {
                Id = id
            };

            var queryHandler = new GetAskByIdQueryHandler(null, null, null);
            var validationBehaviour = new ValidationBehaviour<GetAskByIdQuery, AskObject>(new List<GetAskByIdQueryValidator>
            {
                new()
            }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(getAskByIdQuery,
                    CancellationToken.None,
                    () => queryHandler.Handle(getAskByIdQuery, CancellationToken.None)))
                .Should()
                .ThrowAsync<ValidationException>();
        }
        
        #endregion

        #region GetUserAsksQueryHandler

        [Fact]
        public async Task GetUserAsksQueryHandlerShouldNotThrowWhenPropertiesAreNotInitialized()
        {
            var getUserAsksQuery = new GetUserAsksQuery()
            {
                PageSize = 10,
                PageIndex = 1
            };;

            var queryHandler = new GetUserAsksQueryHandler(null, null, null);
            var validationBehaviour = new ValidationBehaviour<GetUserAsksQuery, PaginatedList<AskObject>>(new List<GetUserAsksQueryValidator>
            {
                new()
            }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(getUserAsksQuery,
                    CancellationToken.None,
                    () => queryHandler.Handle(getUserAsksQuery, CancellationToken.None)))
                .Should()
                .NotThrowAsync<ValidationException>();
        }
        
        [Fact]
        public async Task GetUserAsksQueryHandlerShouldNotThrowWhenPropertiesAreInitialized()
        {
            var getUserAsksQuery = new GetUserAsksQuery()
            {
                PageSize = 100,
                PageIndex = 2
            };

            var queryHandler = new GetUserAsksQueryHandler(null, null, null);
            var validationBehaviour = new ValidationBehaviour<GetUserAsksQuery, PaginatedList<AskObject>>(new List<GetUserAsksQueryValidator>
            {
                new()
            }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(getUserAsksQuery,
                    CancellationToken.None,
                    () => queryHandler.Handle(getUserAsksQuery, CancellationToken.None)))
                .Should()
                .NotThrowAsync<ValidationException>();
        }
        
        [Theory]
        [InlineData(0, 0)]
        [InlineData(10, 0)]
        [InlineData(0, -1)]
        [InlineData(-1, -1)]
        [InlineData(0, 10)]
        public async Task GetUserAsksQueryHandlerShouldThrowWhenPropertiesAreInvalid(int pageSize, int pageIndex)
        {
            var getUserAsksQuery = new GetUserAsksQuery()
            {
                PageSize = pageSize,
                PageIndex = pageIndex
            };

            var queryHandler = new GetUserAsksQueryHandler(null, null, null);
            var validationBehaviour = new ValidationBehaviour<GetUserAsksQuery, PaginatedList<AskObject>>(new List<GetUserAsksQueryValidator>
            {
                new()
            }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(getUserAsksQuery,
                    CancellationToken.None,
                    () => queryHandler.Handle(getUserAsksQuery, CancellationToken.None)))
                .Should()
                .ThrowAsync<ValidationException>();
        }

        #endregion
    }
}