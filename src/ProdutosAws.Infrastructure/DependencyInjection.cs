namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            var region = RegionEndpoint.GetBySystemName(configuration["AWS:Region"]);

            ConfigureAws(services, configuration, region);
            ConfigureDbContext(services, region);

            return services;
        }
    }

    private static void ConfigureAws(IServiceCollection services, IConfiguration configuration, RegionEndpoint region)
    {
        services.Configure<AwsSettings>(configuration.GetSection("AWS"));
        services.AddSingleton<IAmazonS3>(_ => new AmazonS3Client(region));
        services.AddSingleton<IAmazonSecretsManager>(_ => new AmazonSecretsManagerClient(region));
    }

    private static void ConfigureDbContext(IServiceCollection services, RegionEndpoint region)
    {
        string connectionString;

        try
        {
            Console.WriteLine("Obtendo Connection String do Secrets Manager...");

            using var client = new AmazonSecretsManagerClient(region);

            var response = client.GetSecretValueAsync(
                new GetSecretValueRequest { SecretId = "ProdutosAws/Database" }
            ).GetAwaiter().GetResult();

            Console.WriteLine("Secret obtido.");

            using var json = JsonDocument.Parse(response.SecretString);

            connectionString = json.RootElement
                                   .GetProperty("ConnectionString")
                                   .GetString()
                               ?? throw new InvalidOperationException(
                                   "ConnectionString não encontrada no Secret.");

            Console.WriteLine("Connection String carregada.");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao obter Secret:");
            Console.WriteLine(ex);
            throw;
        }

        services.AddDbContext<ProductsAwsDbContext>(options =>
        {
            options.UseSqlServer(connectionString, sql =>
            {
                sql.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null
                );
            });
        });

        services.AddScoped<IProductAwsDbContext>(provider => provider.GetRequiredService<ProductsAwsDbContext>());
        services.AddScoped<IFileStorageService, AwsS3StorageService>();
    }
}
