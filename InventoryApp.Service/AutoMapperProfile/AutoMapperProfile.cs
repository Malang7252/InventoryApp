using AutoMapper;
using InventoryApp.Core.Entities;
using InventoryApp.Service.DTO;

namespace InventoryApp.Service.AutoMapperProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
        }
    }
}
