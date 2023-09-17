using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using RecruitmentManager.Domain.Models;
using System.Net;

namespace RecruitmentManager.Api.Middlewares
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionFilter> _logger;

        public ExceptionFilter(ILogger<ExceptionFilter> logger) => _logger = logger;

        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var statusCode = exception switch
            {
                DomainValidationException => HttpStatusCode.BadRequest,
                DomainNotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError,
            };

            if (statusCode == HttpStatusCode.InternalServerError)
                _logger.LogError(exception, exception.Message);

            context.Result = new JsonResult(new { error = exception.Message }) { StatusCode = (int)statusCode };
        }
    }
}