using AutoMapper;
using CaseMms.Shared.EventBus;
using Catalog.API.Infrastructure.DTOs;
using Catalog.API.Infrastructure.Entities;

namespace Catalog.API.Infrastructure.Mapper;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        // DTOs
        CreateMap<CreateProductDto, Product>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.StockQuantity))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));

        CreateMap<UpdateProductDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.StockQuantity))
            .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));

        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.StockQuantity))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name));

        // Events
        CreateMap<Product, ProductChangedEvent>()
            .ForMember(dest => dest.MessageId, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.StockQuantity))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.IsLive, opt => opt.MapFrom(src => src.StockQuantity >= src.Category.MinStockQuantity));

        CreateMap<Product, ProductDeletedEvent>()
            .ForMember(dest => dest.MessageId, opt => opt.MapFrom(src => Guid.NewGuid()))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id));
    }
}