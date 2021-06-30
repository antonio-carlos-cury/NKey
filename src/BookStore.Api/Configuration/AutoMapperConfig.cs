using AutoMapper;
namespace BookStore.Api.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig() 
        {
            CreateMap<Domain.Entities.Author, Domain.Models.AuthorViewModel>().ReverseMap();
            CreateMap<Domain.Entities.Category, Domain.Models.CategoryViewModel>().ReverseMap();
            CreateMap<Domain.Entities.Book, Domain.Models.BookViewModel>().ReverseMap();
        }

    }
}
