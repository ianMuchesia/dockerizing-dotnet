using Todo.Contracts.Interfaces;

namespace Todo.Middleware;

public class JwtMiddleware
{
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IAuthService authService)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        
        if (token != null)
        {
            var userId = authService.ValidateJwtToken(token);
            if (userId != null)
            {
                // Attach user ID to context for later use
                context.Items["UserId"] = userId;
            }
        }

        await _next(context);
    }
}