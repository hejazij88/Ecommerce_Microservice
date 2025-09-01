namespace Catalog.Api.Dtos;

public class Product_DTO
{
    public record ProductDto(string Id, string Name, string Description, decimal Price, int Stock);


    public record CreateProductDto(string Name, string Description, decimal Price, int Stock);
}