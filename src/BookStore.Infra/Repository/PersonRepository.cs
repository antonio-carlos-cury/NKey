using BookStore.Domain.Entities;
using BookStore.Domain.Interfaces;
using BookStore.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Infra.Repository
{
    public class PersonRepository : Repository<Person>, IPersonRepository
    {
        public PersonRepository(BookStoreDbContext context) : base(context) { }

        public async Task<IEnumerable<Person>> GetPersonsByTypeAsync(PersonType personType)
        {
            return await SearchAsync(p => p.Type == personType);
        }
    }
}
