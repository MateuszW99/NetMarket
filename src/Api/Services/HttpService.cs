using System;
using System.Security.Claims;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Api.Services
{
    public class HttpService : IHttpService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HttpService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            if (_httpContextAccessor.HttpContext?.User == null)
            {
                return String.Empty;
            }

            return _httpContextAccessor.HttpContext.User.FindFirstValue("id") ?? string.Empty;
        }
    }
}