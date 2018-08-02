using _180727_DBA_21_Ex10.Dtos;
using AutoMapper;
using ProductShop.Models;

namespace _180727_DBA_21_Ex10
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<UserDto_02, User>();
            CreateMap<ProductDto_02, Product>();
            CreateMap<CategoryDto_02, Category>();
        }
    }
}
