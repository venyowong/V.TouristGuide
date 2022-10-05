using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace V.TouristGuide.Server.Attributes
{
    public class UserIdAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Items.ContainsKey("userId"))
            {
                context.Result = new StatusCodeResult(401);
                return;
            }
            var userId = context.HttpContext.Items["userId"]?.ToString();
            if (string.IsNullOrWhiteSpace(userId))
            {
                context.Result = new StatusCodeResult(401);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}
