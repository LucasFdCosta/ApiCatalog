using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Catalog.Api.Filters
{
    public class ApiLoggingFilter : IActionFilter
    {
        private readonly ILogger<ApiLoggingFilter> _logger;

        public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("### Executing -> OnActionExecuting");
            _logger.LogInformation("#####################################################");
            _logger.LogInformation($"{DateTime.Today.ToLongDateString()}");
            _logger.LogInformation($"ModelState: {context.ModelState.IsValid}");
            _logger.LogInformation("#####################################################");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("### Executing -> OnActionExecuted");
            _logger.LogInformation("#####################################################");
            _logger.LogInformation($"{DateTime.Today.ToLongDateString()}");
            _logger.LogInformation($"Status code: {context.HttpContext.Response.StatusCode}");
            _logger.LogInformation("#####################################################");
        }
    }
}