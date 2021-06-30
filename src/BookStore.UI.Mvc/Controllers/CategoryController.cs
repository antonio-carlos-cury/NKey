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
        public CategoryController(INotificator notificator, IConfiguration configuration)
        {
            _categoryService = new CategoryService(notificator, configuration.GetSection("BookStoreApiUrl").Value);
        }

        [HttpGet("lista-de-categorias")]
        public async Task<ActionResult> Index()
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index", "auth");

            LoginResponseViewModel currentUser = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);
            ViewBag.UserEmail = currentUser.UserToken.Email;
            var response = await _categoryService.GetAllAsync(currentUser.AccessToken);

            if (response.success)
                return View(JsonConvert.DeserializeObject<IEnumerable<CategoryViewModel>>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpPost("lista-de-categorias")]
        public async Task<ActionResult> Index(string search)
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index", "auth");

            LoginResponseViewModel currentUser = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);
            ViewBag.UserEmail = currentUser.UserToken.Email;
            ViewBag.SearchedText = search;
            DefaultApiResponseViewModel response;

            if (!string.IsNullOrEmpty(search))
                response = await _categoryService.SearchAsync(currentUser.AccessToken, search);
            else
                response = await _categoryService.GetAllAsync(currentUser.AccessToken);

            if (response.success)
                return View(JsonConvert.DeserializeObject<IEnumerable<CategoryViewModel>>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpGet("inserir")]
        public ActionResult Insert()
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index", "auth");

            return View(new CategoryViewModel() { Id = Guid.NewGuid(), IsActive = true });
        }

        [HttpPost("inserir")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Insert(CategoryViewModel category)
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index", "auth");

            LoginResponseViewModel currentUser = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);
            var response = await _categoryService.InsertAsync(currentUser.AccessToken, category);

            return Json(response);
        }

        [HttpGet("editar/{id:guid}")]
        public async Task<ActionResult> Edit(Guid id)
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index", "auth");

            LoginResponseViewModel currentUser = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);
            var response = await _categoryService.GetByIdAsync(currentUser.AccessToken, id);

            if (response.success)
                return View(JsonConvert.DeserializeObject<CategoryViewModel>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpPost("editar")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CategoryViewModel author)
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index", "auth");

            LoginResponseViewModel currentUser = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);
            var response = await _categoryService.UpdateAsync(currentUser.AccessToken, author);

            return Json(response);
        }

        [HttpGet("remover/{id:guid}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index", "auth");

            LoginResponseViewModel currentUser = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);
            var response = await _categoryService.GetByIdAsync(currentUser.AccessToken, id);

            if (response.success)
                return View(JsonConvert.DeserializeObject<CategoryViewModel>(response.data.ToString()));
            else
                return View("Error", response.errors);
        }

        [HttpPost("remover")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(CategoryViewModel author)
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index", "auth");

            LoginResponseViewModel currentUser = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);
            var response = await _categoryService.DeleteAsync(currentUser.AccessToken, author);

            return Json(response);
        }

        [HttpGet("pesquisar")]
        public async Task<ActionResult> Search(string query)
        {
            if (!IsAuthenticated(HttpContext))
                return RedirectToAction("index", "auth");

            LoginResponseViewModel currentUser = JsonConvert.DeserializeObject<LoginResponseViewModel>(UserData);
            ViewBag.UserEmail = currentUser.UserToken.Email;

            DefaultApiResponseViewModel response = await _categoryService.SearchAsync(currentUser.AccessToken, query);

            return Json(JsonConvert.SerializeObject(response.data));
        }
    }
}
