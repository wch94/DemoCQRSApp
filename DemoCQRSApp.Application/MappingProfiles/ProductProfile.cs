using AutoMapper;
using Application.DTOs;
using Domain.Entities;

namespace Application.MappingProfiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>();
    }
}