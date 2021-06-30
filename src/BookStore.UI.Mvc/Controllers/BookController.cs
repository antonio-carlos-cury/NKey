using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
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
        private LoginResponseViewModel _userData;
        public BookController(INotificator notificator, IConfiguration configuration) : base(configuration)
        {
            _bookService = new BookService(notificator, _bookStoreApiUrl);
        }

        [HttpGet("lista-de-livros")]
        public async Task<ActionResult> Index()
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index", "auth");

            var response = await _bookService.GetAllAsync(_userData.AccessToken);

            if (response.success)
                return View(JsonConvert.DeserializeObject<IEnumerable<BookViewModel>>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpPost("lista-de-livros")]
        public async Task<ActionResult> Index(string search)
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index", "auth");

            ViewBag.SearchedText = search;
            DefaultApiResponseViewModel response;

            if (!string.IsNullOrEmpty(search))
                response = await _bookService.SearchAsync(_userData.AccessToken, search);
            else
                response = await _bookService.GetAllAsync(_userData.AccessToken);

            if (response.success)
                return View(JsonConvert.DeserializeObject<IEnumerable<BookViewModel>>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpGet("inserir")]
        public ActionResult Insert()
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index", "auth");

            BookViewModel model = new() { Id = Guid.NewGuid(), IsActive = true };
            return View(model);
        }

        [HttpPost("inserir")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(BookViewModel book)
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index", "auth");


            var response = await _bookService.InsertAsync(_userData.AccessToken, book);

            return Json(response);
        }

        [HttpGet("editar/{id:guid}")]
        public async Task<ActionResult> Edit(Guid id)
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index", "auth");

            var response = await _bookService.GetByIdAsync(_userData.AccessToken, id);

            if (response.success)
                return View(JsonConvert.DeserializeObject<BookViewModel>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpPost("editar")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(BookViewModel author)
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index", "auth");


            var response = await _bookService.UpdateAsync(_userData.AccessToken, author);

            return Json(response);
        }

        [HttpGet("remover/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index", "auth");

            var response = await _bookService.GetByIdAsync(_userData.AccessToken, id);

            if (response.success)
                return View(JsonConvert.DeserializeObject<BookViewModel>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpPost("remover")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(BookViewModel author)
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index", "auth");

            var response = await _bookService.DeleteAsync(_userData.AccessToken, author);

            return Json(response);
        }
    }
}
