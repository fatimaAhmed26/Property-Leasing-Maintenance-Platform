using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using PropertyLeasing.API.Data;
using System.Security.Claims;

namespace PropertyLeasingSystem.Filters
{
    public class NotificationCountFilter : IAsyncResultFilter
    {
        private readonly ApplicationDbContext _context;

        public NotificationCountFilter(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            if (context.Controller is Controller controller &&
                context.HttpContext.User.Identity?.IsAuthenticated == true)
            {
                var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (!string.IsNullOrEmpty(userId))
                {
                    var count = await _context.Notifications
                        .CountAsync(n => n.UserId == userId && !n.IsRead);
                    controller.ViewBag.UnreadNotificationCount = count;
                }
            }

            await next();
        }
    }
}
