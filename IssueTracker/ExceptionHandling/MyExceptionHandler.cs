using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace IssueTracker.ExceptionHandling
{
    public class MyExceptionHandler(ILogger<MyExceptionHandler> logger, IProblemDetailsService problemDetailsService) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            logger.LogError(exception, "An unhandled exception occurred LOL.");

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            try
            {
                await problemDetailsService.WriteAsync(new ProblemDetailsContext
                {
                    HttpContext = httpContext,
                    ProblemDetails = new ProblemDetails
                    {
                        Status = StatusCodes.Status500InternalServerError,
                        Title = "An unexpected error occurred.",
                    }
                }
                );
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "PROBLEM DETAILS WRITE FAILED");
                throw;
            }

            return true;
        }
    }
}
