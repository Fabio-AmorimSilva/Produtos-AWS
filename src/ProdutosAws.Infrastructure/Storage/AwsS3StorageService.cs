namespace ProdutosAws.Infrastructure.Storage;

public class AwsS3StorageService(
    IAmazonS3 s3,
    IOptions<AwsSettings> settings
) : IFileStorageService
{
    private readonly string _bucketName = settings.Value.BucketName;

    public async Task<string> UploadAsync(string key, Stream stream)
    {
        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = stream
        };

        await s3.PutObjectAsync(request);

        return key;
    }

    public async Task<Stream> DownloadAsync(string path)
    {
        var response = await s3.GetObjectAsync(_bucketName, path);

        return response.ResponseStream;
    }

    public string GenerateUrl(string key)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = key,
            Expires = DateTime.UtcNow.AddMinutes(10)
        };

        return s3.GetPreSignedURL(request);
    }

    public async Task DeleteAsync(string path)
    {
        await s3.DeleteObjectAsync(_bucketName, path);
    }
}