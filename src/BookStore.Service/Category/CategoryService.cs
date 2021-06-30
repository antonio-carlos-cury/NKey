using BookStore.Domain.Helpers;
using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using BookStore.Service.Core;
using System;
using System.Threading.Tasks;

namespace BookStore.Service.Category
{

    public class CategoryService : BaseService
    {
        protected readonly string _bookStoreApiUrl;
        public CategoryService(INotificator notificator, string bookStoreApiUrl) : base(notificator)
        {
            _bookStoreApiUrl = bookStoreApiUrl;
        }

        public async Task<int> CountAll(string accessToken)
        {
            using (HttpHelper http = new(bookStoreApiUrl: _bookStoreApiUrl))
            {
                var head = new System.Net.WebHeaderCollection { { "Authorization", $"Bearer {accessToken}" } };
                var response = await http.GetAsync<DefaultApiResponseViewModel>("categorias/total", null, headers: head);
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
                return await http.GetAsync<DefaultApiResponseViewModel>("categorias/obter-todos", null, headers: head);
            };
        }

        public async Task<DefaultApiResponseViewModel> GetByIdAsync(string accessToken, Guid Id)
        {
            using (HttpHelper http = new(bookStoreApiUrl: _bookStoreApiUrl))
            {
                var head = new System.Net.WebHeaderCollection { { "Authorization", $"Bearer {accessToken}" } };
                return await http.GetAsync<DefaultApiResponseViewModel>($"categorias/obter-por-id/{Id}", null, headers: head);
            };
        }

        public async Task<DefaultApiResponseViewModel> InsertAsync(string accessToken, CategoryViewModel category)
        {
            using (HttpHelper http = new(bookStoreApiUrl: _bookStoreApiUrl))
            {
                var head = new System.Net.WebHeaderCollection { { "Authorization", $"Bearer {accessToken}" } };
                return await http.PostAsync<DefaultApiResponseViewModel>($"categorias/inserir", category, headers: head);
            };
        }

        public async Task<DefaultApiResponseViewModel> UpdateAsync(string accessToken, CategoryViewModel category)
        {
            using (HttpHelper http = new(bookStoreApiUrl: _bookStoreApiUrl))
            {
                var head = new System.Net.WebHeaderCollection { { "Authorization", $"Bearer {accessToken}" } };
                return await http.PostAsync<DefaultApiResponseViewModel>($"categorias/atualizar", category, headers: head);
            };
        }

        public async Task<DefaultApiResponseViewModel> DeleteAsync(string accessToken, CategoryViewModel category)
        {
            using (HttpHelper http = new(bookStoreApiUrl: _bookStoreApiUrl))
            {
                var head = new System.Net.WebHeaderCollection { { "Authorization", $"Bearer {accessToken}" } };
                return await http.PostAsync<DefaultApiResponseViewModel>($"categorias/remover", category, headers: head);
            };
        }

        public async Task<DefaultApiResponseViewModel> SearchAsync(string accessToken, string textToSearch)
        {
            using (HttpHelper http = new(bookStoreApiUrl: _bookStoreApiUrl))
            {
                var head = new System.Net.WebHeaderCollection { { "Authorization", $"Bearer {accessToken}" } };
                return await http.GetAsync<DefaultApiResponseViewModel>($"categorias/pesquisar/{textToSearch}", null, headers: head);
            };

        }
    }
}
