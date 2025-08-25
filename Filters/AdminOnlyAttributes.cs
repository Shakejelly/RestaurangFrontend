using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RestaurangFrontend.Filters
{
    public class AdminOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;
            var isAdmin = httpContext.Session.GetString("IsAdmin");
            if (isAdmin != "true")
            {
                context.Result = new RedirectToActionResult("Login", "Admin", null);
            }
        }
    }
}
