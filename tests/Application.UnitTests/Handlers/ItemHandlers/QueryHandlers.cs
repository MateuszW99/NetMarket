using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Behaviours;
using Application.Common.Models;
using Application.Handlers.ItemHandlers;
using Application.Models.ApiModels.Items.Queries;
using Application.Models.DTOs;
using FluentAssertions;
using FluentValidation;
using Xunit;

namespace Application.UnitTests.Handlers.ItemHandlers
{
    public class QueryHandlers
    {
        #region GetItemByIdQueryHandler

        [Fact]
        public async Task GetItemByIdQueryHandlerShouldNotThrowWhenPropertiesAreValid()
        {
            var getItemByIdQuery = new GetItemByIdQuery(Guid.NewGuid().ToString());

            var queryHandler = new GetItemByIdQueryHandler(null, null);
            var validationBehaviour = new ValidationBehaviour<GetItemByIdQuery, ItemObject>(new List<GetItemByIdQueryValidator>
            {
                new()
            }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(
                    getItemByIdQuery,
                    CancellationToken.None,
                    () => queryHandler.Handle(getItemByIdQuery, CancellationToken.None)))
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
        public async Task GetItemByIdQueryHandlerShouldThrowWhenPropertiesAreInvalid(string id)
        {
            var getItemByIdQuery = new GetItemByIdQuery(id);

            var queryHandler = new GetItemByIdQueryHandler(null, null);
            var validationBehaviour = new ValidationBehaviour<GetItemByIdQuery, ItemObject>(new List<GetItemByIdQueryValidator>
            {
                new()
            }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(
                    getItemByIdQuery,
                    CancellationToken.None,
                    () => queryHandler.Handle(getItemByIdQuery, CancellationToken.None)))
                .Should()
                .ThrowAsync<ValidationException>();
        }
        
        #endregion

        #region GetItemsQueryHandler

        [Fact]
        public async Task GetItemsQueryHandlerShouldNotThrowWhenPropertiesAreValid()
        {
            var query = new GetItemsQuery()
            {
                PageIndex = 1,
                PageSize = 10,
                SearchQuery = new()
            };

            var queryHandler = new GetItemsQueryHandler(null, null);
            var validationBehaviour = new ValidationBehaviour<GetItemsQuery, PaginatedList<ItemObject>>(
                new List<GetItemsQueryValidator>
                {
                    new()
                }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(
                    query, 
                    CancellationToken.None,
                    () => queryHandler.Handle(query, CancellationToken.None)))
                .Should()
                .NotThrowAsync<ValidationException>();
        }
        
        [Theory]
        [InlineData(-1, -1, null)]
        [InlineData(-1, 0, null)]
        [InlineData(0, 1, null)]
        public async Task GetItemsQueryHandlerShouldThrowWhenPropertiesAreInvalid(int pageNumber, int pageSize, SearchItemsQuery searchItemsQuery)
        {
            var query = new GetItemsQuery()
            {
                PageIndex = pageNumber,
                PageSize = pageSize,
                SearchQuery = searchItemsQuery
            };

            var queryHandler = new GetItemsQueryHandler(null, null);
            var validationBehaviour = new ValidationBehaviour<GetItemsQuery, PaginatedList<ItemObject>>(
                new List<GetItemsQueryValidator>
                {
                    new()
                }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(
                    query, 
                    CancellationToken.None,
                    () => queryHandler.Handle(query, CancellationToken.None)))
                .Should()
                .ThrowAsync<ValidationException>();
        }
        
        #endregion

        #region GetItemsWithCategoryQuery

        [Fact]
        public async Task GetItemsWithCategoryQueryHandlerShouldNotThrowWhenPropertiesAreValid()
        {
            var query = new GetItemsWithCategoryQuery()
            {
                Category = "Sneakers",
                PageIndex = 1,
                PageSize = 10
            };

            var queryHandler = new GetItemsWithCategoryQueryHandler(null, null);
            var validationBehaviour = new ValidationBehaviour<GetItemsWithCategoryQuery, PaginatedList<ItemObject>>(new List<GetItemsWithCategoryQueryValidator>
            {
                new()
            }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(
                    query,
                    CancellationToken.None,
                    () => queryHandler.Handle(query, CancellationToken.None)))
                .Should()
                .NotThrowAsync<ValidationException>();
        }
        
        [Theory]
        [InlineData("", 0, 0)]
        [InlineData(null, -2, 0)]
        [InlineData("Sneakers", 0, 0)]
        [InlineData("Sneakers", 0, -1)]
        public async Task GetItemsWithCategoryQueryHandlerShouldNotThrowWhenPropertiesAreInvalid(string category, int pageIndex, int pageSize)
        {
            var query = new GetItemsWithCategoryQuery()
            {
                Category = category,
                PageIndex = pageIndex,
                PageSize = pageSize
            };

            var queryHandler = new GetItemsWithCategoryQueryHandler(null, null);
            var validationBehaviour = new ValidationBehaviour<GetItemsWithCategoryQuery, PaginatedList<ItemObject>>(new List<GetItemsWithCategoryQueryValidator>
            {
                new()
            }, null);

            await FluentActions.Invoking(() => validationBehaviour.Handle(
                    query,
                    CancellationToken.None,
                    () => queryHandler.Handle(query, CancellationToken.None)))
                .Should()
                .ThrowAsync<ValidationException>();
        }
        
        #endregion
    }
}