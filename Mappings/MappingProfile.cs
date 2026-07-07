using AutoMapper;
using EcommerceMVC.DTOs;
using EcommerceMVC.Models;
using EcommerceMVC.ViewModels;

namespace EcommerceMVC.Mappings;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

        CreateMap<Product, ProductFormViewModel>().ReverseMap();

        CreateMap<CartItem, CartItemDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl));

        CreateMap<Cart, CartDto>();
    }
}
