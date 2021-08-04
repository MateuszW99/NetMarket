using System;
using System.Collections.Generic;
using Application.Common.Exceptions;
using FluentAssertions;
using FluentValidation.Results;
using Xunit;

namespace Application.UnitTests.Common.Validators
{
    public class CommonValidationCases
    {
        [Fact]
        public void ShouldCreateEmptyDictionary()
        {
            var actual = new ValidationException().Errors;

            actual.Keys.Should().BeEquivalentTo(Array.Empty<string>());
        }

        [Fact]
        public void ShouldCreateOneElementDictionaryForSingleValidationFailure()
        {
            var failures = new List<ValidationFailure>
            {
                new("Age", "must be over 18.")
            };

            var actual = new ValidationException(failures).Errors;

            actual.Keys.Should().HaveCount(1);
            actual.Keys.Should().Contain(new[] { "Age" });
            actual["Age"].Should().BeEquivalentTo("must be over 18.");
        }

        [Fact]
        public void ShouldCreateManyElementDictionaryForManyValidationFailures()
        {
            var failures = new List<ValidationFailure>
            {
                new("Age", "must be 18 or older"),
                new("Age", "must be 25 or younger"),
                new("Password", "must contain at least 8 characters"),
                new("Password", "must contain a digit"),
                new("Password", "must contain upper case letter"),
                new("Password", "must contain lower case letter"),
            };

            var actual = new ValidationException(failures).Errors;

            actual.Keys.Should().BeEquivalentTo(new[] { "Password", "Age" });

            actual["Age"].Should().BeEquivalentTo(new[]
            {
                "must be 25 or younger",
                "must be 18 or older",
            });

            actual["Password"].Should().BeEquivalentTo(new[]
            {
                "must contain lower case letter",
                "must contain upper case letter",
                "must contain at least 8 characters",
                "must contain a digit",
            });
        }
    }
}