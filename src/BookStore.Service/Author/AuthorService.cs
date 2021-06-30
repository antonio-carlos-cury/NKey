using BookStore.Domain.Helpers;
using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using BookStore.Service.Core;
using System;
using System.Threading.Tasks;

namespace BookStore.Service.Author
{
    public class AuthorService : BaseService
    {
        protected readonly string _bookStoreApiUrl;
        public AuthorService(INotificator notificator, string bookStoreApiUrl) : base(notificator) 
        {
            _bookStoreApiUrl = bookStoreApiUrl;
        }


        public async Task<int> CountAllAsync(string accessToken)
        {
            using (HttpHelper http = new(bookStoreApiUrl: _bookStoreApiUrl))
            {
                var head = new System.Net.WebHeaderCollection { { "Authorization", $"Bearer {accessToken}" } };
                var response = await http.GetAsync<DefaultApiResponseViewModel>("autores/total", null, headers: head);
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
                return await http.GetAsync<DefaultApiResponseViewModel>("autores/obter-todos", null, headers: head);
            };
        }

        public async Task<DefaultApiResponseViewModel> GetByIdAsync(string accessToken, Guid Id)
        {
            using (HttpHelper http = new(bookStoreApiUrl: _bookStoreApiUrl))
            {
                var head = new System.Net.WebHeaderCollection { { "Authorization", $"Bearer {accessToken}" } };
                return await http.GetAsync<DefaultApiResponseViewModel>($"autores/obter-por-id/{Id}", null, headers: head);
            };
        }

        public async Task<DefaultApiResponseViewModel> InsertAsync(string accessToken, AuthorViewModel author)
        {
            using (HttpHelper http = new(bookStoreApiUrl: _bookStoreApiUrl))
            {
                var head = new System.Net.WebHeaderCollection { { "Authorization", $"Bearer {accessToken}" } };
                return await http.PostAsync<DefaultApiResponseViewModel>($"autores/inserir", author, headers: head);
            };
        }

        public async Task<DefaultApiResponseViewModel> UpdateAsync(string accessToken, AuthorViewModel author)
        {
            using (HttpHelper http = new(bookStoreApiUrl: _bookStoreApiUrl))
            {
                var head = new System.Net.WebHeaderCollection { { "Authorization", $"Bearer {accessToken}" } };
                return await http.PostAsync<DefaultApiResponseViewModel>($"autores/atualizar", author, headers: head);
            };
        }

        public async Task<DefaultApiResponseViewModel> DeleteAsync(string accessToken, AuthorViewModel author)
        {
            using (HttpHelper http = new(bookStoreApiUrl: _bookStoreApiUrl))
            {
                var head = new System.Net.WebHeaderCollection { { "Authorization", $"Bearer {accessToken}" } };
                return await http.PostAsync<DefaultApiResponseViewModel>($"autores/remover", author, headers: head);
            };
        }

        public async Task<DefaultApiResponseViewModel> SearchAsync(string accessToken, string textToSearch)
        {
            using (HttpHelper http = new(bookStoreApiUrl: _bookStoreApiUrl))
            {
                var head = new System.Net.WebHeaderCollection { { "Authorization", $"Bearer {accessToken}" } };
                return await http.GetAsync<DefaultApiResponseViewModel>($"autores/pesquisar/{textToSearch}", null, headers: head);
            };

        }

        public Task<bool> ExistsBookByAuthorId(string accessToken, Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
