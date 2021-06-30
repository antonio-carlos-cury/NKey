using BookStore.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BookStore.UI.Mvc.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly string _bookStoreApiUrl;
        public BaseController(IConfiguration config)
        {
            _bookStoreApiUrl = config.GetSection("BookStoreApiUrl").Value;            
        }
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
            ViewBag.ApiUrl = _bookStoreApiUrl.Remove(_bookStoreApiUrl.Length - 1, 1);
            return true;
        }


        private string UserData { get; set; }
        protected bool IsAuthenticated(HttpContext context)
        {
            if (!context.Session.IsAvailable || !context.Session.TryGetValue("UserData", out byte[] CurrentUserData))
                return false;

            UserData = System.Text.Encoding.Default.GetString(CurrentUserData);
            return true;

        }
    }
}
