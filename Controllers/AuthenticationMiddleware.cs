using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace UserManagementAPI.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Example: Check for a header "Authorization: Bearer <token>"
            var authHeader = context.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                var result = JsonSerializer.Serialize(new { error = "Unauthorized: Missing or invalid token." });
                await context.Response.WriteAsync(result);
                return;
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();

            // Validate the token (for demonstration, accept any non-empty token)
            if (string.IsNullOrWhiteSpace(token))
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                var result = JsonSerializer.Serialize(new { error = "Unauthorized: Invalid token." });
                await context.Response.WriteAsync(result);
                return;
            }

            // If token is valid, proceed
            await _next(context);
        }
    }
}
