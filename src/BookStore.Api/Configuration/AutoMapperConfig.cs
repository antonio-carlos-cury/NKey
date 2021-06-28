using AutoMapper;
namespace BookStore.Api.Configuration
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig() 
        {
            CreateMap<Domain.Entities.Author, Domain.Models.AuthorViewModel>().ReverseMap();
        }

    }
}
