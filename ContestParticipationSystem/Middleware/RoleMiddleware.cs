using System.Security.Claims;
using ContestParticipationSystem.Models;

namespace ContestParticipationSystem.Middleware
{
    public class RoleMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var role = context.User.FindFirst(ClaimTypes.Role)?.Value;

                if (context.Request.Path.Value.Contains("vip"))
                {
                    if (role != UserRole.VIP.ToString() && role != UserRole.ADMIN.ToString())
                    {
                        context.Response.StatusCode = 403;
                        await context.Response.WriteAsync("VIP access only");
                        return;
                    }
                }
            }

            await _next(context);
        }
    }
}