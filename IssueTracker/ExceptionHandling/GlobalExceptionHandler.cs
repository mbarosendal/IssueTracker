using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.ExceptionHandling
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "An unhandled exception occurred.");

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            int status = StatusCodes.Status500InternalServerError;
            string title = "An unexpected error occurred.";

            bool writeSuccess = await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = new ProblemDetails
                {
                    Status = status,
                    Title = title,
                }
            });

            // Return normal JSON without problem details formatting if the problem details service fails to write
            if (!writeSuccess)
            {
                httpContext.Response.ContentType = "application/json";
                await httpContext.Response.WriteAsJsonAsync(new
                {
                    Status = status,
                    Title = title
                }, cancellationToken);
            }

            return true;
        }
    }
}
