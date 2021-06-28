﻿using BookStore.Domain.Entities;
using BookStore.Domain.Interfaces;
using BookStore.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.Infra.Repository
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(BookStoreDbContext context) : base(context) { }
        public async Task<IEnumerable<Book>> GetBooksByAuthorIdAsync(Guid AuthorId)
        {
            return await Db.Books.AsNoTracking().Include(b => b.Author).Where(b => b.Author.Id == AuthorId).ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorNameAsync(string authorName)
        {
            return await Db.Books.AsNoTracking().Include(b => b.Author).Where(b => b.Author.Name.Equals(authorName, StringComparison.InvariantCultureIgnoreCase)).ToListAsync();
        }
    }
}
