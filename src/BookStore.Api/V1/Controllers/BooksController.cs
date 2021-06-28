using AutoMapper;
using BookStore.Api.Controllers;
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
    [Route("api/v{version:apiVersion}/livros")]
    public class BooksController : MainController
    {

        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public BooksController(IBookRepository bookRepository, IUser user, INotificator notificator, IMapper mapper) : base(notificator, user)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        [HttpGet("obter-todos")]
        public async Task<ActionResult> GetAll()
        {
            return CustomResponse(_mapper.Map<IEnumerable<BookViewModel>>(await _bookRepository.GetAllAsync()));
        }

        [HttpGet("obter-por-id/{id:guid}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            return CustomResponse(_mapper.Map<BookViewModel>(await _bookRepository.GetByIdAsync(id)));
        }

        [HttpGet("total")]
        public async Task<ActionResult> Count()
        {
            return CustomResponse(await _bookRepository.CountAsync());
        }

    }
}
