﻿using System;
using System.Net.Http;
using Application.IntegrationTests.Helpers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests
{
    public abstract class IntegrationTest : IDisposable
    {
        protected readonly ISender _mediator;
        protected readonly CustomWebApplicationFactory _factory;
        protected readonly HttpClient _client;

        protected static readonly string ApiBaseAddress = "/api";
        protected static readonly string ItemsAddress = "items";
        
        protected IntegrationTest()
        {
            _factory = new CustomWebApplicationFactory();
            _client = _factory.CreateClient();
            _client.BaseAddress = new Uri("https://localhost", UriKind.Absolute);
            //Creates mediator service
            var scope = _factory.Services.CreateScope();
            _mediator = scope.ServiceProvider.GetRequiredService<ISender>();
        }
        
        public void Dispose()
        {
            DbHelper.DeleteDatabase(_factory);
        }
    }
}