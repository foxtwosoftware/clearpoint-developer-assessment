using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace TodoList.Api.ExceptionFilters
{
    public class ApiErrorExceptionFilter : IExceptionFilter
    {
        private readonly ProblemDetailsFactory _problemDetailsFactory;

        public ApiErrorExceptionFilter(ProblemDetailsFactory problemDetailsFactory) => _problemDetailsFactory = problemDetailsFactory;

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            var httpContext = context.HttpContext;

            var problemDetails = _problemDetailsFactory.CreateProblemDetails(httpContext, statusCode: 500, title: "API error", detail: exception.Message);
            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status
            };

            context.ExceptionHandled = true;
        }

    }
}