using AutoMapper;
using Goba_Store.Models;
using Goba_Store.View_Models;

namespace Goba_Store
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
