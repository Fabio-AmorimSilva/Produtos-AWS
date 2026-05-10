namespace ProdutosAws.Infrastructure.Storage;

public record AwsSettings
{
    public string BucketName { get; init; } = null!;
    public string Region { get; init; } = null!;
}