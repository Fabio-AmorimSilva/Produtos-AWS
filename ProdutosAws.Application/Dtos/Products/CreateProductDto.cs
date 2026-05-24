namespace ProdutosAzure.Application.Dtos.Products;

public record CreateProductDto
{
    public string Name { get; set; } = string.Empty;
    public ProductCategory ProductCategory { get; set; }
}