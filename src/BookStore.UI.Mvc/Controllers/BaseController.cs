using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.UI.Mvc.Controllers
{
    public abstract class BaseController : Controller
    {
        public string UserData { get; set; }
        protected bool IsAuthenticated(HttpContext context)
        {
            if (!context.Session.IsAvailable || !context.Session.TryGetValue("UserData", out byte[] CurrentUserData))
                return false;

            UserData = System.Text.Encoding.Default.GetString(CurrentUserData);
            return true;

        }
    }
}
