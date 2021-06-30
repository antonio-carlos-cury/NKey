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
    [Route("livros")]
    public class BookController : BaseController
    {
        private readonly BookService _bookService;
        public BookController(INotificator notificator, IConfiguration configuration)
        {
            _bookService = new BookService(notificator, configuration.GetSection("BookStoreApiUrl").Value);
        }

        [HttpGet("lista-de-livros")]
        public async Task<ActionResult> Index()
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index", "auth");

            LoginResponseViewModel currentUser = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);
            ViewBag.UserEmail = currentUser.UserToken.Email;
            var response = await _bookService.GetAllAsync(currentUser.AccessToken);

            if (response.success)
                return View(JsonConvert.DeserializeObject<IEnumerable<BookViewModel>>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpPost("lista-de-livros")]
        public async Task<ActionResult> Index(string search)
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index", "auth");

            LoginResponseViewModel currentUser = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);
            ViewBag.UserEmail = currentUser.UserToken.Email;
            ViewBag.SearchedText = search;
            DefaultApiResponseViewModel response;

            if (!string.IsNullOrEmpty(search))
                response = await _bookService.SearchAsync(currentUser.AccessToken, search);
            else
                response = await _bookService.GetAllAsync(currentUser.AccessToken);

            if (response.success)
                return View(JsonConvert.DeserializeObject<IEnumerable<BookViewModel>>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpGet("inserir")]
        public ActionResult Insert()
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index", "auth");

            BookViewModel model = new() { Id = Guid.NewGuid(), IsActive = true };
            return View(model);
        }

        [HttpPost("inserir")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(BookViewModel book)
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index", "auth");

            LoginResponseViewModel currentUser = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);
            var response = await _bookService.InsertAsync(currentUser.AccessToken, book);

            return Json(response);
        }

        [HttpGet("editar/{id:guid}")]
        public async Task<ActionResult> Edit(Guid id)
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index", "auth");

            LoginResponseViewModel currentUser = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);
            var response = await _bookService.GetByIdAsync(currentUser.AccessToken, id);

            if (response.success)
                return View(JsonConvert.DeserializeObject<BookViewModel>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpPost("editar")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BookViewModel author)
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index", "auth");

            LoginResponseViewModel currentUser = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);
            var response = await _bookService.UpdateAsync(currentUser.AccessToken, author);

            return Json(response);
        }

        [HttpGet("remover/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index", "auth");

            LoginResponseViewModel currentUser = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);
            var response = await _bookService.GetByIdAsync(currentUser.AccessToken, id);

            if (response.success)
                return View(JsonConvert.DeserializeObject<BookViewModel>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpPost("remover")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(BookViewModel author)
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index", "auth");

            LoginResponseViewModel currentUser = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);
            var response = await _bookService.DeleteAsync(currentUser.AccessToken, author);

            return Json(response);
        }
    }
}
