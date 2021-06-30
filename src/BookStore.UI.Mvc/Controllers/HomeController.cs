using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using BookStore.Service.Author;
using BookStore.Service.Authorization;
using BookStore.Service.Book;
using BookStore.Service.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BookStore.UI.Mvc.Controllers
{
    [Route("inicio")]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        private LoginResponseViewModel _userData;
        private readonly AuthorService _authorService;
        private readonly BookService _bookService;
        private readonly AuthService _authService;
        private readonly CategoryService _categoryService;
        public HomeController(ILogger<HomeController> logger, INotificator notificator, IConfiguration configuration) : base(configuration)
        {
            _logger = logger;
            _authorService = new AuthorService(notificator, _bookStoreApiUrl);
            _bookService = new BookService(notificator, _bookStoreApiUrl);
            _authService = new AuthService(notificator, _bookStoreApiUrl);
            _categoryService = new CategoryService(notificator, _bookStoreApiUrl);
        }

        [HttpGet("painel")]
        public async Task<IActionResult> Index()
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index", "auth");
            
            ViewBag.TotalAuthorNumber = await _authorService.CountAllAsync(_userData.AccessToken);
            ViewBag.TotalBookNumber = await _bookService.CountAll(_userData.AccessToken);
            ViewBag.TotalUserNumber = await _authService.CountAll(_userData.AccessToken);
            ViewBag.TotalCategoryNumber = await _categoryService.CountAll(_userData.AccessToken);

            _logger.LogInformation($"Usuário {_userData.AccessToken} logado no sistema");

            return View();

        }
    }
}
