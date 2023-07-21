using AutoMapper;
using Goba_Store.Application.View_Models;
using Goba_Store.Models;

namespace Goba_Store.Application
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CategoryViewModel,Category>().ReverseMap();
            CreateMap<ProductViewModel, Product>().ReverseMap();
        }
    }
}
