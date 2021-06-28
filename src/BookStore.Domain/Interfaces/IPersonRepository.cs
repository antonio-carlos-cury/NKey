using BookStore.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookStore.Domain.Interfaces
{
    public interface IPersonRepository : IRepository<Person>
    {
        Task<IEnumerable<Person>> GetPersonsByTypeAsync(PersonType personType);
    }
}
