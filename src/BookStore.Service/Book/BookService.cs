using BookStore.Domain.Helpers;
using BookStore.Domain.Interfaces;
using BookStore.Domain.Models;
using BookStore.Service.Core;
using System.Threading.Tasks;

namespace BookStore.Service.Book
{
    public class BookService : BaseService
    {
        protected readonly string _bookStoreApiUrl;
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

    }
}
