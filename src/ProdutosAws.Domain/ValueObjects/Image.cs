namespace ProdutosAws.Domain.ValueObjects;

public class Image
{
    public string Key { get; private set; }
    public string Path { get; private set; }

    public Image(string path)
    {
        Path = path;
        Key = $"{Guid.NewGuid()}-{path}";
    }
}