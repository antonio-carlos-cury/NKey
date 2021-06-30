using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Domain.Entities
{
    public class Book : Entity
    {

        public int Code { get; set; }
        
        public string Name { get; set; }

        public string Isbn { get; set; }

        public short ReleaseYear { get; set; }

        public string Preface { get; set; }

        public short TotalChaptersNumbers { get; set; }

        public short TotalPagesNumbers { get; set; }

        
        
        /*EF Relations*/
        [ForeignKey("AuthorId")]
        public Author Author { get; set; }
        public Guid AuthorId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
        public Guid CategoryId { get; set; }

    }
}
