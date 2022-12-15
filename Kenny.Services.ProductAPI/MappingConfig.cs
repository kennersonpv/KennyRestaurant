using AutoMapper;
using Kenny.Services.ProductAPI.Models;
using Kenny.Services.ProductAPI.Models.Dto;
using System.Diagnostics.CodeAnalysis;

namespace Kenny.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDto, Product>();
                config.CreateMap<Product, ProductDto>();

                //Does the same thing
                //config.CreateMap<ProductDto, Product>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
