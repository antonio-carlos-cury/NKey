using System.Collections.Generic;

namespace BookStore.Domain.Entities
{
    public class Author : Entity
    {

        public int Code { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }


        /*EF Relations*/
        public IEnumerable<Book> Books { get; set; }



    }
}
