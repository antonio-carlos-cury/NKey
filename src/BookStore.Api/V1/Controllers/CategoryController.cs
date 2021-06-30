using AutoMapper;
using BookStore.Api.Controllers;
using BookStore.Domain.Entities;
using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Api.V1.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/categorias")]
    public class CategoryController : MainController
    {

        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IUser user, INotificator notificator, IMapper mapper) : base(notificator, user)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet("obter-todos")]
        public async Task<ActionResult> GetAllAsync()
        {
            return CustomResponse(_mapper.Map<IEnumerable<CategoryViewModel>>(await _categoryRepository.GetAllAsync()));
        }

        [HttpGet("pesquisar/{text}")]
        public async Task<ActionResult> SearchAsync(string text)
        {
            return CustomResponse(_mapper.Map<IEnumerable<CategoryViewModel>>(await _categoryRepository.SearchAsync(a => a.Code.ToString().Contains(text) || a.Name.Contains(text))));
        }

        [HttpGet("obter-por-id/{id:guid}")]
        public async Task<ActionResult> GetByIdAsync(Guid id)
        {
            return CustomResponse(_mapper.Map<CategoryViewModel>(await _categoryRepository.GetByIdAsync(id)));
        }

        [HttpGet("total")]
        public async Task<ActionResult> CountAsync()
        {
            return CustomResponse(await _categoryRepository.CountAsync());
        }

        [HttpPost("inserir")]
        public async Task<ActionResult> InsertAsync(CategoryViewModel categoryViewModel)
        {
            Category category = _mapper.Map<Category>(categoryViewModel);
            await _categoryRepository.InsertAsync(category);
            return CustomResponse("Categoria inserida com sucesso!");
        }

        [HttpPost("atualizar")]
        public async Task<ActionResult> UpdateAsync(CategoryViewModel categoryViewModel)
        {
            Category category = _mapper.Map<Category>(categoryViewModel);
            await _categoryRepository.UpdateAsync(category);
            return CustomResponse("Categoria atualizada com sucesso!");
        }

        [HttpPost("remover")]
        public async Task<ActionResult> DeleteAsync(CategoryViewModel categoryViewModel)
        {
            Category category = _mapper.Map<Category>(categoryViewModel);
            await _categoryRepository.DeleteAsync(category.Id);
            return CustomResponse("Categoria removida com sucesso!");
        }
    }
}
