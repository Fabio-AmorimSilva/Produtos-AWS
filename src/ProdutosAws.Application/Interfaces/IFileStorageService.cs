namespace ProdutosAws.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> UploadAsync(string key, Stream stream);
    Task<Stream> DownloadAsync(string path);
    string GenerateUrl(string key);
    Task DeleteAsync(string path);
}