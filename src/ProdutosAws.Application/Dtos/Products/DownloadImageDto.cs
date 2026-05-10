namespace ProdutosAws.Application.Dtos.Products;

public record DownloadImageDto
{
    public Guid ProductId { get; init; }
    public string Key { get; init; } = string.Empty;
}