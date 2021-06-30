using AutoMapper;
using BookStore.Api.Controllers;
using BookStore.Domain.Entities;
using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BookStore.Api.V1.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/autores")]
    public class AuthorsController : MainController
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public AuthorsController(IAuthorRepository authorRepository, IUser user, INotificator notificator, IMapper mapper) : base(notificator, user)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        [HttpGet("obter-todos")]
        public async Task<ActionResult> GetAllAsync()
        {
            return CustomResponse(_mapper.Map<IEnumerable<AuthorViewModel>>(await _authorRepository.GetAllAsync()));
        }

        [HttpGet("pesquisar/{text}")]
        public async Task<ActionResult> SearchAsync(string text)
        {
            return CustomResponse(_mapper.Map<IEnumerable<AuthorViewModel>>(await _authorRepository.SearchAsync(a => a.Code.ToString().Contains(text) || a.Name.Contains(text) || a.Email.Contains(text))));
        }

        [HttpGet("obter-por-id/{id:guid}")]
        public async Task<ActionResult> GetByIdAsync(Guid id)
        {
            return CustomResponse(_mapper.Map<AuthorViewModel>(await _authorRepository.GetByIdAsync(id)));
        }

        [HttpGet("total")]
        public async Task<ActionResult> CountAsync()
        {
            return CustomResponse(await _authorRepository.CountAsync());
        }

        [HttpPost("inserir")]
        public async Task<ActionResult> InsertAsync(AuthorViewModel authorViewModel)
        {
            Author author = _mapper.Map<Author>(authorViewModel);
            await _authorRepository.InsertAsync(author);
            return CustomResponse("Autor inserido com sucesso!");
        }

        [HttpPost("atualizar")]
        public async Task<ActionResult> UpdateAsync(AuthorViewModel authorViewModel)
        {
            Author author = _mapper.Map<Author>(authorViewModel);
            await _authorRepository.UpdateAsync(author);
            return CustomResponse("Autor atualizado com sucesso!");
        }

        [HttpPost("remover")]
        public async Task<ActionResult> DeleteAsync(AuthorViewModel authorViewModel)
        {
            Author author = _mapper.Map<Author>(authorViewModel);
            await _authorRepository.DeleteAsync(author.Id);
            return CustomResponse("Autor removido com sucesso!");
        }
    }
}
