using System;
using System.Net.Http;
using Application.Common.Interfaces;
using Application.IntegrationTests.Helpers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests
{
    public static class Address 
    {
        public static readonly string ApiBase = "/api";
        public static readonly string Items = "items";
        public static readonly string Category = "category";
        public static readonly string UserSettings = "user";
        public static readonly string AdminPanel = "adminPanel";
        public static readonly string SupervisorPanel = "supervisorPanel";
        public static readonly string Orders = "orders";
    }
    
    public abstract class IntegrationTest : IDisposable
    {
        protected readonly ISender _mediator;
        protected readonly CustomWebApplicationFactory _factory;
        protected readonly HttpClient _client;
        protected readonly IIdentityService _identityService;
        

        
        protected IntegrationTest()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost", UriKind.Absolute);
            //Creates mediator service
            var scope = _factory.Services.CreateScope();
            _mediator = scope.ServiceProvider.GetRequiredService<ISender>();
            _identityService = scope.ServiceProvider.GetRequiredService<IIdentityService>();
        }

        public void Dispose()
        {
            DbHelper.DeleteDatabase(_factory);
        }
    }
}