﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Behaviours;
using Application.Common.Interfaces;
using Application.Handlers.UserSettingsHandlers;
using Application.Models.ApiModels.UserSettings.Commands;
using Domain.Enums;
using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Application.UnitTests.Handlers.UserSettingsHandlers
{
    public class UserSettingsCommandHandlersTests
    {
        private readonly Mock<IHttpService> _httpServiceMock;
        private readonly Mock<IUserSettingsService> _userSettingsServiceMock;
        private readonly Mock<ILogger<UpdateUserSettingsQueryHandler>> _loggerMock;

        public UserSettingsCommandHandlersTests()
        {
            _httpServiceMock = new Mock<IHttpService>();
            _userSettingsServiceMock = new Mock<IUserSettingsService>();
            _loggerMock = new Mock<ILogger<UpdateUserSettingsQueryHandler>>();

            _httpServiceMock.Setup(x => x.GetUserId()).Returns(Guid.NewGuid().ToString());
            _userSettingsServiceMock.Setup(x =>
                    x.UpdateUserSettingsAsync(It.IsAny<Guid>(), It.IsAny<UpdateUserSettingsCommand>(),
                        CancellationToken.None))
                .Verifiable();
        }

        [Fact]
        public async Task UpdateUserSettingsCommandHandlerShouldNotThrowWhenPropertiesAreValid()
        {
            var updateUserSettingsCommand = new UpdateUserSettingsCommand()
            {
                SellerLevel = 0,
                SalesCompleted = 0,
                PaypalEmail = "user@test.com",
                BillingStreet = "test",
                BillingAddressLine1 = "test",
                BillingAddressLine2 = "test",
                BillingZipCode = "12345",
                BillingCountry = "test",
                ShippingStreet = "test",
                ShippingAddressLine1 = "test",
                ShippingAddressLine2 = "test",
                ShippingZipCode = "12-345",
                ShippingCountry = "test",
            };

            var commandHandler = new UpdateUserSettingsQueryHandler(_userSettingsServiceMock.Object, _httpServiceMock.Object,
                _loggerMock.Object);
            var validationBehaviour = new ValidationBehaviour<UpdateUserSettingsCommand, MediatR.Unit>(
                new List<UpdateUserSettingsCommandValidator>()
                {
                    new()
                }, null);

            await FluentActions.Invoking(() =>
                    validationBehaviour.Handle(
                        updateUserSettingsCommand,
                        CancellationToken.None,
                        () => commandHandler.Handle(updateUserSettingsCommand, CancellationToken.None)))
                .Should()
                .NotThrowAsync<ValidationException>();
        }

        [Theory]
        [MemberData(nameof(UserSettings))]
        public async Task UpdateUserSettingsCommandHandlerShouldThrowWhenOneOrMorePropertiesAreInvalid(
            SellerLevel sellerLevel, int salesCompleted, string paypalEmail,
            string billingStreet, string billingAddressLine1, string billingAddressLine2,
            string billingZipCode, string billingCountry, string shippingStreet, string shippingAddressLine1,
            string shippingAddressLine2, string shippingZipCode, string shippingCountry)
        {
            var updateUserSettingsCommand = new UpdateUserSettingsCommand()
            {
                SellerLevel = sellerLevel,
                SalesCompleted = salesCompleted,
                PaypalEmail = paypalEmail,
                BillingStreet = billingStreet,
                BillingAddressLine1 = billingAddressLine1,
                BillingAddressLine2 = billingAddressLine2,
                BillingZipCode = billingZipCode,
                BillingCountry = billingCountry,
                ShippingStreet = shippingStreet,
                ShippingAddressLine1 = shippingAddressLine1,
                ShippingAddressLine2 = shippingAddressLine2,
                ShippingZipCode = shippingZipCode,
                ShippingCountry = shippingCountry
            };

            var commandHandler = new UpdateUserSettingsQueryHandler(_userSettingsServiceMock.Object, _httpServiceMock.Object,
                _loggerMock.Object);
            var validationBehaviour = new ValidationBehaviour<UpdateUserSettingsCommand, MediatR.Unit>(
                new List<UpdateUserSettingsCommandValidator>()
                {
                    new()
                }, null);

            await FluentActions.Invoking(() =>
                    validationBehaviour.Handle(
                        updateUserSettingsCommand,
                        CancellationToken.None,
                        () => commandHandler.Handle(updateUserSettingsCommand, CancellationToken.None)))
                .Should()
                .ThrowAsync<ValidationException>();
        }

        public static IEnumerable<object[]> UserSettings => new List<object[]>
        {
            new object[]
            {
                0, 0, "user@test.com", "street", "address line 1", "address line 2", "123-45", "USA", "street",
                "address line 1", " address line 2", "1234-5", "USA"
            },
            new object[]
            {
                0, 2, "usertest.com", "street", "address line 1", "address line 2", "12345", "USA", "street",
                "address line 1", " address line 2", "12345", "USA"
            },
            new object[]
            {
                5, 0, "user@test.com", "street", "address line 1", "address line 2", "12345", "USA", "street",
                "address line 1", " address line 2", "12345", "USA"
            },
            new object[]
            {
                0, -2, "user@test.com", "street", "address line 1", "address line 2", "12345", "USA", "street",
                "address line 1", " address line 2", "12345", "USA"
            },
        };
    }
}