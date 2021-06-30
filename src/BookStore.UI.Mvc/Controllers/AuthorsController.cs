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
        private LoginResponseViewModel _userData;
        public AuthorsController(INotificator notificator, IConfiguration configuration) : base(configuration)
        {
            _authorService = new AuthorService(notificator, _bookStoreApiUrl);
            _bookService = new BookService(notificator, _bookStoreApiUrl);
        }

        [HttpGet("lista-de-autores")]
        public async Task<ActionResult> Index()
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index","auth");

            var response = await _authorService.GetAllAsync(_userData.AccessToken);
            
            if (response.success)
                return View(JsonConvert.DeserializeObject<IEnumerable<AuthorViewModel>>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpPost("lista-de-autores")]
        public async Task<ActionResult> Index(string searchAuthor)
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index","auth");

            ViewBag.SearchedText = searchAuthor;
            DefaultApiResponseViewModel response;

            if (!string.IsNullOrEmpty(searchAuthor))
                response = await _authorService.SearchAsync(_userData.AccessToken, searchAuthor);
            else
                response = await _authorService.GetAllAsync(_userData.AccessToken);

            if (response.success)
                return View(JsonConvert.DeserializeObject<IEnumerable<AuthorViewModel>>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpGet("inserir")]
        public ActionResult Insert()
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index","auth");

            return View(new AuthorViewModel() { Id = Guid.NewGuid(), IsActive = true});
        }


        [HttpPost("inserir")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(AuthorViewModel author)
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index","auth");

            var response = await _authorService.InsertAsync(_userData.AccessToken, author);

            return Json(response);
        }

        [HttpGet("detalhes/{id:guid}")]
        public async Task<ActionResult> Details(Guid id)
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index","auth");

            var response = await _authorService.GetByIdAsync(_userData.AccessToken, id);

            if (response.success)
                return View(JsonConvert.DeserializeObject<AuthorViewModel>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpGet("editar/{id:guid}")]
        public async Task<ActionResult> Edit(Guid id)
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index","auth");


            var response = await _authorService.GetByIdAsync(_userData.AccessToken, id);

            if (response.success)
                return View(JsonConvert.DeserializeObject<AuthorViewModel>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpPost("editar")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AuthorViewModel author)
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index","auth");

            var response = await _authorService.UpdateAsync(_userData.AccessToken, author);

            return Json(response);
        }

        [HttpGet("remover/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index","auth");


            var response = await _authorService.GetByIdAsync(_userData.AccessToken, id);

            if (response.success)
                return View(JsonConvert.DeserializeObject<AuthorViewModel>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpPost("remover")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(AuthorViewModel author)
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index","auth");

            var AuthorHasBook = await _bookService.GetBookByAuthorId(_userData.AccessToken, author.Id);
            if (AuthorHasBook.success)
            {
                AuthorHasBook.success = false;
                AuthorHasBook.errors = new List<string>() {"Exitem livros vinculados a este autor, para remove-lo, primeiramente altere os livros." };
                return Json(AuthorHasBook);
            }

            var response = await _authorService.DeleteAsync(_userData.AccessToken, author);
            return Json(response);
        }

        [HttpGet("pesquisar")]
        public async Task<ActionResult> Search(string query)
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index", "auth");
            
            DefaultApiResponseViewModel response = await _authorService.SearchAsync(_userData.AccessToken, query);

            return Json(JsonConvert.SerializeObject(response.data));
        }

    }
}
