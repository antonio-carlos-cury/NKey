
namespace BookStore.Domain.Interfaces
{
    public interface IBookService
    {
        public bool Validate(Entities.Book book);
    }
}
