namespace BookStore.Domain.Entities
{
    public class Book : Entity
    {

        public int Code { get; set; }
        
        public string Name { get; set; }

        public string Isbn { get; set; }

        public short ReleaseYear { get; set; }
        
        /*EF Relations*/
        public Author Author { get; set; }

        public Category Category { get; set; }

    }
}
