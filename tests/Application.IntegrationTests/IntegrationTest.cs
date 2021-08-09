using System;
using Application.IntegrationTests.Helpers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests
{
    public abstract class IntegrationTest : IDisposable
    {
        protected readonly ISender _mediator;
        
        protected readonly CustomWebApplicationFactory _factory;
        
        protected IntegrationTest()
        {
            _factory = new CustomWebApplicationFactory();
            _factory.CreateClient();

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