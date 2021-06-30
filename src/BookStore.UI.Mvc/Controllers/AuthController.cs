using BookStore.Domain.Helpers;
using BookStore.Domain.Interfaces;
using BookStore.Service.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BookStore.UI.Mvc.Controllers
{
    [Route("login")]
    public class AuthController : BaseController
    {
        protected readonly INotificator _notificator;
        protected readonly string BookStoreApiUrl;
        public AuthController(INotificator notificator, IConfiguration configuration)
        {
            _notificator = notificator;
            BookStoreApiUrl = configuration.GetSection("BookStoreApiUrl").Value;
        }

        [HttpGet("entrar")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("nova-conta")]
        public async Task<IActionResult> RegistrarSe(string Email, string Password, string ConfirmPassword)
        {
            AuthService authService = new(_notificator, BookStoreApiUrl);
            var response = await authService.AccountRegister(Email, Password, ConfirmPassword);

            if (response.success)
            {
                HttpContext.Session.Set("UserData", System.Text.Encoding.Default.GetBytes(JsonConvert.SerializeObject(response.data)));
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["Errors"] = response.errors.ToDefaultJSON();
                ViewBag.Error = "Não foi possível criar sua conta";
                ViewBag.ErrorType = "register";
                return View("Index");
            }
        }


        [HttpPost("entrar")]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            AuthService authService = new(_notificator, BookStoreApiUrl);
            var response = await authService.AccountLogin(Email, Password);

            if (response.success)
            {
                HttpContext.Session.Set("UserData", System.Text.Encoding.Default.GetBytes(JsonConvert.SerializeObject(response.data)));
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["Errors"] = response.errors.ToDefaultJSON();
                ViewBag.Error = "Não foi possível fazer login";
                ViewBag.ErrorType = "login";
                return View("Index");
            }
        }

        [HttpGet("sair")]
        public IActionResult Logoff()
        {
            if (IsAuthenticated(HttpContext))
                HttpContext.Session.Clear();

            return RedirectToAction("index", "Auth");
        }
    }
}
