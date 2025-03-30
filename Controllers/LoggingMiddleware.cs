using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace UserManagementAPI.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoggingMiddleware> _logger;

        public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var request = context.Request;
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("Incoming Request: {method} {url}", request.Method, request.Path);

            await _next(context);

            stopwatch.Stop();
            var response = context.Response;
            _logger.LogInformation("Outgoing Response: {statusCode} - Elapsed: {elapsed}ms", 
                response.StatusCode, stopwatch.ElapsedMilliseconds);
        }
    }
}
