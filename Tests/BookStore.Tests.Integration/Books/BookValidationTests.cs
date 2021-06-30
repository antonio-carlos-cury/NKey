using BookStore.Domain.Entities;
using BookStore.Domain.Notifications;
using BookStore.Service.Book;
using Xunit;

namespace BookStore.Tests.Integration.Books
{
    public class BookServiceValitations
    {
        [Fact(DisplayName = "Garante que um livro tenha o código ISBN informado.")]
        public void book_without_isbn_should_be_false()
        {
            BookService bs = new(new Notificator());
            Book book = MakeFakeBook();
            bs.Validate(book);
            
            Assert.All(bs._notificator.GetAll(), item => Assert.Contains("O campo ISBN precisa ter 13 caracteres", item.Message));
        }

        private Book MakeFakeBook()
        {
            return new Book()
            {
                Author = new Author(),
                Category = new Category(),
                Name = "Um livro utilizado para teste",
                Isbn = "444-asd-444",
                Code = 1
            };
        }
    }
}
