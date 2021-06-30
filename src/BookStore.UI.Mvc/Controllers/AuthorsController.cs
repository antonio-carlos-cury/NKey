using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using BookStore.Service.Author;
using BookStore.Service.Book;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.UI.Mvc.Controllers
{
    [Route("autores")]
    public class AuthorsController : BaseController
    {

        private readonly AuthorService _authorService;
        private readonly BookService _bookService;
        public AuthorsController(INotificator notificator, IConfiguration configuration)
        {
            _authorService = new AuthorService(notificator, configuration.GetSection("BookStoreApiUrl").Value);
            _bookService = new BookService(notificator, configuration.GetSection("BookStoreApiUrl").Value);
        }

        [HttpGet("lista-de-autores")]
        public async Task<ActionResult> Index()
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index","auth");

            LoginResponseViewModel currentUser = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);
            ViewBag.UserEmail = currentUser.UserToken.Email;
            var response = await _authorService.GetAllAsync(currentUser.AccessToken);
            
            if (response.success)
                return View(JsonConvert.DeserializeObject<IEnumerable<AuthorViewModel>>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpPost("lista-de-autores")]
        public async Task<ActionResult> Index(string searchAuthor)
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index","auth");

            LoginResponseViewModel currentUser = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);
            ViewBag.UserEmail = currentUser.UserToken.Email;
            ViewBag.SearchedText = searchAuthor;
            DefaultApiResponseViewModel response;

            if (!string.IsNullOrEmpty(searchAuthor))
                response = await _authorService.SearchAsync(currentUser.AccessToken, searchAuthor);
            else
                response = await _authorService.GetAllAsync(currentUser.AccessToken);

            if (response.success)
                return View(JsonConvert.DeserializeObject<IEnumerable<AuthorViewModel>>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpGet("inserir")]
        public ActionResult Insert()
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index","auth");

            return View(new AuthorViewModel() { Id = Guid.NewGuid(), IsActive = true});
        }


        [HttpPost("inserir")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(AuthorViewModel author)
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index","auth");

            LoginResponseViewModel currentUser = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);
            var response = await _authorService.InsertAsync(currentUser.AccessToken, author);

            return Json(response);
        }

        [HttpGet("detalhes/{id:guid}")]
        public async Task<ActionResult> Details(Guid id)
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index","auth");

            LoginResponseViewModel currentUser = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);
            var response = await _authorService.GetByIdAsync(currentUser.AccessToken, id);

            if (response.success)
                return View(JsonConvert.DeserializeObject<AuthorViewModel>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpGet("editar/{id:guid}")]
        public async Task<ActionResult> Edit(Guid id)
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index","auth");

            LoginResponseViewModel currentUser = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);
            var response = await _authorService.GetByIdAsync(currentUser.AccessToken, id);

            if (response.success)
                return View(JsonConvert.DeserializeObject<AuthorViewModel>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpPost("editar")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AuthorViewModel author)
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index","auth");

            LoginResponseViewModel currentUser = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);
            var response = await _authorService.UpdateAsync(currentUser.AccessToken, author);

            return Json(response);
        }

        [HttpGet("remover/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index","auth");

            LoginResponseViewModel currentUser = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);
            var response = await _authorService.GetByIdAsync(currentUser.AccessToken, id);

            if (response.success)
                return View(JsonConvert.DeserializeObject<AuthorViewModel>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpPost("remover")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(AuthorViewModel author)
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index","auth");

            LoginResponseViewModel currentUser = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);

            var AuthorHasBook = await _bookService.GetBookByAuthorId(currentUser.AccessToken, author.Id);
            if (AuthorHasBook.success)
            {
                AuthorHasBook.success = false;
                AuthorHasBook.errors = new List<string>() {"Exitem livros vinculados a este autor, para remove-lo, primeiramente altere os livros." };
                return Json(AuthorHasBook);
            }

            var response = await _authorService.DeleteAsync(currentUser.AccessToken, author);
            return Json(response);
        }

        [HttpGet("pesquisar")]
        public async Task<ActionResult> Search(string query)
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index", "auth");

            LoginResponseViewModel currentUser = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);
            ViewBag.UserEmail = currentUser.UserToken.Email;
            
            DefaultApiResponseViewModel response = await _authorService.SearchAsync(currentUser.AccessToken, query);

            return Json(JsonConvert.SerializeObject(response.data));
        }

    }
}
