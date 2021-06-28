using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookStore.UI.Mvc.Extensions
{
    public class RequiredAuthenticationAttribute : ActionFilterAttribute
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Session.IsAvailable || !context.HttpContext.Session.TryGetValue("UserData", out _))
                context.Result = new RedirectToRouteResult("login");
            
            base.OnActionExecuting(context);
        }
    }
}
