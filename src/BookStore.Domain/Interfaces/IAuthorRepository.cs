using BookStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Domain.Interfaces
{
    public interface IAuthorRepository : IRepository<Author>
    {
        Task<IEnumerable<Author>> GetAuthorsByBookAsync(Guid BookId);
    }
}
