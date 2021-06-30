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
    [Route("api/v{version:apiVersion}/livros")]
    public class BooksController : MainController
    {

        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly IBookService _bookService;

        public BooksController(IBookRepository bookRepository, 
            IUser user, 
            INotificator notificator,
            IMapper mapper,
            IAuthorRepository authorRepository,
            ICategoryRepository categoryRepository,
            IBookService bookService) : base(notificator, user)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _bookService = bookService;
        }

        [HttpGet("obter-todos")]
        public async Task<ActionResult> GetAllAsync(bool IsCompleteSearch)
        {
            if (!IsCompleteSearch)
                return CustomResponse(_mapper.Map<IEnumerable<BookViewModel>>(await _bookRepository.GetAllAsync()));
            else
            {
                List<BookViewModel> resultsList = new();
                var books = await _bookRepository.GetAllCompleteAsync();
                foreach (Book book in books)
                {
                    resultsList.Add(new BookViewModel()
                    {
                        AuthorId = book.Author.Id,
                        AuthorName = book.Author.Name,
                        CategoryId = book.Category.Id,
                        CategoryName = book.Category.Name,
                        Code = book.Code,
                        Id = book.Id,
                        IsActive = book.IsActive,
                        ISBN = book.Isbn,
                        Name = book.Name,
                        Preface = book.Preface,
                        ReleaseYear = book.ReleaseYear,
                        TotalChaptersNumbers = book.TotalChaptersNumbers,
                        TotalPagesNumbers = book.TotalPagesNumbers
                    });
                }
                return CustomResponse(resultsList);
            }
        }


        [HttpGet("pesquisar/{text}")]
        public async Task<ActionResult> SearchAsync(string text)
        {
            return CustomResponse(_mapper.Map<IEnumerable<BookViewModel>>(await _bookRepository.SearchAsync(a => a.Code.ToString().Contains(text) || a.Name.Contains(text))));
        }

        [HttpGet("obter-por-id/{id:guid}")]
        public async Task<ActionResult> GetByIdAsync(Guid id, bool IsCompleteSearch)
        {
            if (!IsCompleteSearch)
            {
                return CustomResponse(_mapper.Map<BookViewModel>(await _bookRepository.GetByIdAsync(id)));
            }
            else
            {
                var book = await _bookRepository.GetBookCompleteAsync(id);
                var model =  new BookViewModel()
                {
                    AuthorId = book.Author.Id,
                    AuthorName = book.Author.Name,
                    CategoryId = book.Category.Id,
                    CategoryName = book.Category.Name,
                    Code = book.Code,
                    Id = book.Id,
                    IsActive = book.IsActive,
                    ISBN = book.Isbn,
                    Name = book.Name,
                    Preface = book.Preface,
                    ReleaseYear = book.ReleaseYear,
                    TotalChaptersNumbers = book.TotalChaptersNumbers,
                    TotalPagesNumbers = book.TotalPagesNumbers
                };

                return CustomResponse(model);
            }
        }

        [HttpGet("obter-por-autor/{id:guid}")]
        public async Task<ActionResult> GetByAuthorIdAsync(Guid id)
        {
            return CustomResponse(_mapper.Map<IEnumerable<BookViewModel>>(await _bookRepository.GetBooksByAuthorIdAsync(id)));
        }

        [HttpGet("total")]
        public async Task<ActionResult> CountAsync()
        {
            return CustomResponse(await _bookRepository.CountAsync());
        }

        [HttpPost("inserir")]
        public async Task<ActionResult> InsertAsync(BookViewModel bookViewModel)
        {
            Book book = _mapper.Map<Book>(bookViewModel);

            if (!_bookService.Validate(book))
                return CustomResponse(bookViewModel);

            book.Author = await _authorRepository.GetByIdAsync(book.Author.Id);
            book.Category = await _categoryRepository.GetByIdAsync(book.Category.Id);
            await _bookRepository.InsertAsync(book);
            return CustomResponse("Livro inserido com sucesso!");
        }

        [HttpPost("atualizar")]
        public async Task<ActionResult> UpdateAsync(BookViewModel bookViewModel)
        {
            Book book = _mapper.Map<Book>(bookViewModel);
            book.Author = null;
            book.Category = null;

            await _bookRepository.UpdateAsync(book);
            return CustomResponse("Livro atualizado com sucesso!");
        }

        [HttpPost("remover")]
        public async Task<ActionResult> DeleteAsync(BookViewModel bookViewModel)
        {
            Book book = _mapper.Map<Book>(bookViewModel);
            await _bookRepository.DeleteAsync(book.Id);
            return CustomResponse("Livro removido com sucesso!");
        }

    }
}
