using BookStore.Domain.Helpers;
using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using BookStore.Service.Core;
using System.Threading.Tasks;

namespace BookStore.Service.Authorization
{
    public class AuthService : BaseService
    {
        protected readonly string _bookStoreApiUrl;
        public AuthService(INotificator notificator, string bookStoreApiUrl) : base(notificator) 
        {
            _bookStoreApiUrl = bookStoreApiUrl;
        }

        public async Task<DefaultApiResponseViewModel> AccountRegister(string Email, string Password, string ConfirmPassword)
        {
            using (HttpHelper http = new(bookStoreApiUrl: _bookStoreApiUrl))
            {
                RegisterUserViewModel modelContent = new();
                modelContent.Email = Email;
                modelContent.Password = Password;
                modelContent.ConfirmPassword = ConfirmPassword;
                return await http.PostAsync<DefaultApiResponseViewModel>("login/nova-conta", modelContent);
            };
        }

        public async Task<DefaultApiResponseViewModel> AccountLogin(string Email, string Password)
        {
            using (HttpHelper http = new(bookStoreApiUrl: _bookStoreApiUrl))
            {
                LoginUserViewModel modelContent = new();
                modelContent.Email = Email;
                modelContent.Password = Password;
                return await http.PostAsync<DefaultApiResponseViewModel>("login/entrar", modelContent);
            };
        }

        public async Task<int> CountAll(string accessToken)
        {
            using (HttpHelper http = new(bookStoreApiUrl: _bookStoreApiUrl))
            {
                var head = new System.Net.WebHeaderCollection { { "Authorization", $"Bearer {accessToken}" } };
                var response = await http.GetAsync<DefaultApiResponseViewModel>("login/total", null, headers: head);
                int count = 0;

                if (response.success)
                    _ = int.TryParse(response.data.ToString(), out count);

                return count;
            };

        }

    }
}
