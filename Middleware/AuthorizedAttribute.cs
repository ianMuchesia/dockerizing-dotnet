


using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Todo.Middleware;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userId = context.HttpContext.Items["UserId"];
        if (userId == null)
        {
            // Not logged in
            context.Result = new JsonResult(new { message = "Unauthorized" })
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };
        }
    }
}