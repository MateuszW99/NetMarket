using System;
using System.Runtime.Serialization;
using Application.Common.Mappings;
using Application.Models.DTOs;
using AutoMapper;
using Domain.Entities;
using Xunit;

namespace Application.UnitTests.Common.Mappings
{
    public class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });
            _mapper = _configuration.CreateMapper();
        }

        [Fact]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Theory]
        [InlineData(typeof(Ask), typeof(AskObject))]
        [InlineData(typeof(Bid), typeof(BidObject))]
        [InlineData(typeof(Brand), typeof(BrandObject))]
        [InlineData(typeof(Item), typeof(ItemObject))]
        [InlineData(typeof(Size), typeof(SizeObject))]
        [InlineData(typeof(Transaction), typeof(TransactionObject))]
        [InlineData(typeof(UserSettings), typeof(UserSettingsObject))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = GetInstanceOf(source);
            _mapper.Map(instance, source, destination);
        }

        private static object GetInstanceOf(Type type)
        {
            if (type.GetConstructor(Type.EmptyTypes) != null)
                return Activator.CreateInstance(type);

            return FormatterServices.GetUninitializedObject(type);
        }
    }
}