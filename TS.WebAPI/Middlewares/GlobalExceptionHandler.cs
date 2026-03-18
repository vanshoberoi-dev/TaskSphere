using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace TS.API.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            // 1. Log the full error to Serilog
            // This captures the 'Elapsed' time up to the crash point
            _logger.LogError(exception, "Unhandled Exception: {Message} at {Path}",
                exception.Message, httpContext.Request.Path);

            // 2. Create the RFC 7807 'Problem Details' object
            var problemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "An unexpected error occurred",
                Detail = "Internal Server Error. Please trace with ID: " + httpContext.TraceIdentifier,
                Instance = httpContext.Request.Path
            };

            // 3. Return structured JSON to Swagger/Client
            httpContext.Response.StatusCode = problemDetails.Status.Value;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true; // Mark as handled
        }
    }
}