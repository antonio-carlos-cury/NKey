using BookStore.Domain.Helpers;
using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using BookStore.Service.Core;
using BookStore.Service.Validations;
using System;
using System.Threading.Tasks;

namespace BookStore.Service.Book
{
    public class BookService : BaseService, IBookService
    {
        protected readonly string _bookStoreApiUrl;

        public BookService(INotificator notificator) : base(notificator) { }

        public BookService(INotificator notificator, string bookStoreApiUrl) : base(notificator)
        {
            _bookStoreApiUrl = bookStoreApiUrl;
        }
        public async Task<int> CountAll(string accessToken)
        {
            using (HttpHelper http = new(bookStoreApiUrl: _bookStoreApiUrl))
            {
                var head = new System.Net.WebHeaderCollection { { "Authorization", $"Bearer {accessToken}" } };
                var response = await http.GetAsync<DefaultApiResponseViewModel>("livros/total", null, headers: head);
                int count = 0;

                if (response.success)
                    _ = int.TryParse(response.data.ToString(), out count);

                return count;
            };
        }

        public async Task<DefaultApiResponseViewModel> GetAllAsync(string accessToken)
        {
            using (HttpHelper http = new(bookStoreApiUrl: _bookStoreApiUrl))
            {
                var head = new System.Net.WebHeaderCollection { { "Authorization", $"Bearer {accessToken}" } };
                var pr = new HttpParams();
                pr.Add("IsCompleteSearch", true);
                return await http.GetAsync<DefaultApiResponseViewModel>("livros/obter-todos", null, pr, head);
            };
        }

        public async Task<DefaultApiResponseViewModel> GetByIdAsync(string accessToken, Guid Id)
        {
            using (HttpHelper http = new(bookStoreApiUrl: _bookStoreApiUrl))
            {
                var head = new System.Net.WebHeaderCollection { { "Authorization", $"Bearer {accessToken}" } };
                var pr = new HttpParams();
                pr.Add("IsCompleteSearch", true);
                return await http.GetAsync<DefaultApiResponseViewModel>($"livros/obter-por-id/{Id}", null, pr, headers: head);
            };
        }

        public async Task<DefaultApiResponseViewModel> InsertAsync(string accessToken, BookViewModel book)
        {
            using (HttpHelper http = new(bookStoreApiUrl: _bookStoreApiUrl))
            {
                var head = new System.Net.WebHeaderCollection { { "Authorization", $"Bearer {accessToken}" } };
                return await http.PostAsync<DefaultApiResponseViewModel>($"livros/inserir", book, headers: head);
            };
        }

        public async Task<DefaultApiResponseViewModel> UpdateAsync(string accessToken, BookViewModel book)
        {
            using (HttpHelper http = new(bookStoreApiUrl: _bookStoreApiUrl))
            {
                var head = new System.Net.WebHeaderCollection { { "Authorization", $"Bearer {accessToken}" } };
                return await http.PostAsync<DefaultApiResponseViewModel>($"livros/atualizar", book, headers: head);
            };
        }

        public async Task<DefaultApiResponseViewModel> DeleteAsync(string accessToken, BookViewModel book)
        {
            using (HttpHelper http = new(bookStoreApiUrl: _bookStoreApiUrl))
            {
                var head = new System.Net.WebHeaderCollection { { "Authorization", $"Bearer {accessToken}" } };
                return await http.PostAsync<DefaultApiResponseViewModel>($"livros/remover", book, headers: head);
            };
        }

        public async Task<DefaultApiResponseViewModel> SearchAsync(string accessToken, string textToSearch)
        {
            using (HttpHelper http = new(bookStoreApiUrl: _bookStoreApiUrl))
            {
                var head = new System.Net.WebHeaderCollection { { "Authorization", $"Bearer {accessToken}" } };
                return await http.GetAsync<DefaultApiResponseViewModel>($"livros/pesquisar/{textToSearch}", null, headers: head);
            };

        }

        public async Task<DefaultApiResponseViewModel> GetBookByAuthorId(string accessToken, Guid authorId)
        {
            using (HttpHelper http = new(bookStoreApiUrl: _bookStoreApiUrl))
            {
                var head = new System.Net.WebHeaderCollection { { "Authorization", $"Bearer {accessToken}" } };
                return await http.GetAsync<DefaultApiResponseViewModel>($"livros/obter-por-autor/{authorId}", null, headers: head);
            };
        }

        public bool Validate(Domain.Entities.Book book)
        {
            if (ValidateEntity(new BookValidation(), book))
                return true;

            return false;
            
        }
    }
}
