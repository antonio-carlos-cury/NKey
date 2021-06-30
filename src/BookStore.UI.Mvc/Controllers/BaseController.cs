using BookStore.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BookStore.UI.Mvc.Controllers
{
    public abstract class BaseController : Controller
    {

        public bool ValidateUser(out LoginResponseViewModel _userData)
        {
            _userData = new LoginResponseViewModel();
            if (!IsAuthenticated(HttpContext))
            {
                _userData.IsAuthenticated = false;
                return false;
            }

            _userData = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);
            ViewBag.UserEmail = _userData.UserToken.Email;
            ViewBag.AccessToken = _userData.AccessToken;
            return true;
        }


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
