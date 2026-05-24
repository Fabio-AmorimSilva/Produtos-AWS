namespace ProdutosAzure.Application.Dtos.Products;

public record ProductDto
{
    public Guid Id { get; set; }
    public string Name { get; init; } = string.Empty;
    public ProductCategory ProductCategory { get; init; }
}