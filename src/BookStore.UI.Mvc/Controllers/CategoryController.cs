using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using BookStore.Service.Category;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.UI.Mvc.Controllers
{
    [Route("categorias")]
    public class CategoryController : BaseController
    {
        private readonly CategoryService _categoryService;
        private LoginResponseViewModel _userData;
        public CategoryController(INotificator notificator, IConfiguration configuration) : base(configuration)
        {
            _categoryService = new CategoryService(notificator, _bookStoreApiUrl);
        }

        [HttpGet("lista-de-categorias")]
        public async Task<ActionResult> Index()
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index", "auth");

            var response = await _categoryService.GetAllAsync(_userData.AccessToken);

            if (response.success)
                return View(JsonConvert.DeserializeObject<IEnumerable<CategoryViewModel>>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpPost("lista-de-categorias")]
        public async Task<ActionResult> Index(string search)
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index", "auth");

            ViewBag.SearchedText = search;
            DefaultApiResponseViewModel response;

            if (!string.IsNullOrEmpty(search))
                response = await _categoryService.SearchAsync(_userData.AccessToken, search);
            else
                response = await _categoryService.GetAllAsync(_userData.AccessToken);

            if (response.success)
                return View(JsonConvert.DeserializeObject<IEnumerable<CategoryViewModel>>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpGet("inserir")]
        public ActionResult Insert()
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index", "auth");

            return View(new CategoryViewModel() { Id = Guid.NewGuid(), IsActive = true });
        }

        [HttpPost("inserir")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(CategoryViewModel category)
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index", "auth");

            var response = await _categoryService.InsertAsync(_userData.AccessToken, category);

            return Json(response);
        }

        [HttpGet("editar/{id:guid}")]
        public async Task<ActionResult> Edit(Guid id)
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index", "auth");

            var response = await _categoryService.GetByIdAsync(_userData.AccessToken, id);

            if (response.success)
                return View(JsonConvert.DeserializeObject<CategoryViewModel>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpPost("editar")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CategoryViewModel author)
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index", "auth");

            var response = await _categoryService.UpdateAsync(_userData.AccessToken, author);

            return Json(response);
        }

        [HttpGet("remover/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index", "auth");

            var response = await _categoryService.GetByIdAsync(_userData.AccessToken, id);

            if (response.success)
                return View(JsonConvert.DeserializeObject<CategoryViewModel>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpPost("remover")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(CategoryViewModel author)
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index", "auth");

            var response = await _categoryService.DeleteAsync(_userData.AccessToken, author);

            return Json(response);
        }

        [HttpGet("pesquisar")]
        public async Task<ActionResult> Search(string query)
        {
            if (!ValidateUser(out _userData))
                return RedirectToAction("index", "auth");

            DefaultApiResponseViewModel response = await _categoryService.SearchAsync(_userData.AccessToken, query);

            return Json(JsonConvert.SerializeObject(response.data));
        }
    }
}
