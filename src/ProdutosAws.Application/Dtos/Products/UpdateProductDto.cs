namespace ProdutosAws.Application.Dtos.Products;

public record UpdateProductDto
{
    public Guid ProductId { get; init; }
    public string Name { get; init; } = string.Empty;
    public ProductCategory ProductCategory { get; init; }
}