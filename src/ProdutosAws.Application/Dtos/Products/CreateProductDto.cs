namespace ProdutosAws.Application.Dtos.Products;

public record CreateProductDto
{
    public string Name { get; init; } = string.Empty;
    public ProductCategory ProductCategory { get; init; }
}