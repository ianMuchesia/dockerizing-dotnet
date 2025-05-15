using Todo.Contracts.Interfaces;

namespace Todo.Middleware;



public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IAuthService _authService;

        public JwtMiddleware(RequestDelegate next, IAuthService authService)
        {
            _next = next;
            _authService = authService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            
            if (token != null)
            {
                var userId = _authService.ValidateJwtToken(token);
                if (userId != null)
                {
                    // Attach user ID to context for later use
                    context.Items["UserId"] = userId;
                }
            }

            await _next(context);
        }
    }