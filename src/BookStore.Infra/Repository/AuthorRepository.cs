using BookStore.Domain.Entities;
using BookStore.Domain.Interfaces;
using BookStore.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Infra.Repository
{
    public class AuthorRepository : Repository<Author>, IAuthorRepository
    {
        public AuthorRepository(BookStoreDbContext context) : base(context) { }

        public async Task<IEnumerable<Author>> GetAuthorsByBookAsync(Guid BookId)
        {
            return await Db.Authors.AsNoTracking().Include(a => a.Books).Where(a => a.Books.Any(b => b.Id == BookId)).ToListAsync();
        }
    }
}
