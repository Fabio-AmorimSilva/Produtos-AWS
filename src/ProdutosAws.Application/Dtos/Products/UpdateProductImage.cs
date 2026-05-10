namespace ProdutosAws.Application.Dtos.Products;

public record UpdateProductImage
{
    public Guid ProductId { get; init; }
    public string Path { get; init; } = string.Empty;
    public Stream Stream { get; init; } = null!;
}