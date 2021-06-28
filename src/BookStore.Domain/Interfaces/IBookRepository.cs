using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Domain.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(Guid auhorId);
        Task<IEnumerable<Book>> GetBooksByAuthorNameAsync(string authorName);
    }
}
