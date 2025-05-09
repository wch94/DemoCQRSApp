using AutoMapper;
using DemoCQRSApp.Application.DTOs;
using DemoCQRSApp.Domain.Entities;

namespace DemoCQRSApp.Application.MappingProfiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>();
    }
}