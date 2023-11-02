using API.Extensions;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API.Helpers
{
    public class LogUSerActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultCntext = await next();

            if (!resultCntext.HttpContext.User.Identity.IsAuthenticated) return;
             
            var userId = resultCntext.HttpContext.User.GetuserId();

            var uow = resultCntext.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();

            var user = await uow.UserRepository.GetUserByIdAsync(userId);
            user.LastActive = DateTime.UtcNow;
            await uow.Complete();
        }
    }
}