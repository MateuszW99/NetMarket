﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Api.Common
{
    public class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILogger<ApiExceptionFilterAttribute> _logger;
        private readonly IDictionary<Type, Func<ExceptionContext, ObjectResult>> _exceptionHandlers;

        public ApiExceptionFilterAttribute(ILogger<ApiExceptionFilterAttribute> logger)
        {
            _logger = logger;
            _exceptionHandlers = new Dictionary<Type, Func<ExceptionContext, ObjectResult>>()
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException }
            };
        }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            var objectResult = HandleException(context);
            context.ExceptionHandled = true;
            context.Result = new JsonResult(new
            {
                message = objectResult
            });

            _logger.LogInformation($"{context.Exception.GetType().FullName} was handled.");

            await base.OnExceptionAsync(context);
        }

        private ObjectResult HandleException(ExceptionContext context)
        {
            var type = context.Exception.GetType();
            if (_exceptionHandlers.Keys.Contains(type))
            {
                return _exceptionHandlers[type].Invoke(context);
            }

            return HandleUnknownException(context);
        }

        private ObjectResult HandleValidationException(ExceptionContext context)
        {
            var exception = context.Exception as ValidationException;
            var details = new ValidationProblemDetails(exception.Errors);
            return new BadRequestObjectResult(details);
        }

        private ObjectResult HandleNotFoundException(ExceptionContext context)
        {
            var exception = context.Exception as NotFoundException;

            var details = new ProblemDetails()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "The specified resource was not found.",
                Detail = exception?.Message
            };

            return new NotFoundObjectResult(details);
        }

        private ObjectResult HandleUnauthorizedAccessException(ExceptionContext context)
        {
            var exception = context.Exception as UnauthorizedAccessException;

            var details = new ProblemDetails()
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3",
                Title = "The specified resource cannot be accessed.",
                Detail = exception?.Message
            };
            
            return new UnauthorizedObjectResult(details);
        }
        
        private ObjectResult HandleUnknownException(ExceptionContext context)
        {
            var details = new ProblemDetails()
            {
                Status = StatusCodes.Status404NotFound,
                Title = "Something went wrong."
            };
            return new ObjectResult(details);
        }
    }
}