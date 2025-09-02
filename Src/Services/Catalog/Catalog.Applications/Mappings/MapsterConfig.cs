using Catalog.Domain.Entity;
using Catalog.Api.Dtos;
using Mapster;

namespace Catalog.Applications.Mappings;

public static class MapsterConfig
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<Product, Product_DTO.ProductDto>.NewConfig();
        TypeAdapterConfig<Product_DTO.CreateProductDto, Product>.NewConfig()
            .Map(dest => dest.Id, src => Guid.NewGuid().ToString());
    }
}