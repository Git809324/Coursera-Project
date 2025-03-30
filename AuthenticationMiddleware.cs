public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public AuthenticationMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task Invoke(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/api/auth") || 
            context.Request.Path.StartsWithSegments("/health"))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("Authorization", out var authHeader))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Authorization header is missing");
            return;
        }

        var token = authHeader.ToString().Split(" ").Last();
        if (!ValidateToken(token))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid token");
            return;
        }

        await _next(context);
    }

    private bool ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]);
            
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
