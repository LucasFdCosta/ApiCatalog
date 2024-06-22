using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Catalog.Api.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, "An unhandled exception occurred: Status code 500");

            context.Result = new ObjectResult(new
            {
                Message = "A problem occurred while handling your request: Status code 500",
                Error = context.Exception.Message,
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }
}