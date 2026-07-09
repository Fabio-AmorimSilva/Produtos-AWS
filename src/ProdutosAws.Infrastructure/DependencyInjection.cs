namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure(IConfiguration configuration)
        {
            services
                .AddAwsS3(configuration)
                .AddDbContext(configuration);

            return services;
        }

        private IServiceCollection AddDbContext(IConfiguration configuration)
        {
            using var client = new AmazonSecretsManagerClient();

            var response = client
                .GetSecretValueAsync(new GetSecretValueRequest { SecretId = "ProdutosAws/Database" })
                .GetAwaiter()
                .GetResult();

            var secrets = JsonDocument.Parse(response.SecretString);

            var connectionString = secrets.RootElement.GetProperty("ConnectionString").GetString();

            services.AddDbContext<ProductsAwsDbContext>(options =>
                options.UseSqlServer(connectionString)
            );

            services.AddScoped<IProductAwsDbContext>(provider => provider.GetRequiredService<ProductsAwsDbContext>());

            return services;
        }

        private IServiceCollection AddAwsS3(IConfiguration configuration)
        {
            services.Configure<AwsSettings>(
                configuration.GetSection("AWS")
            );

            var regionName = configuration["AWS:Region"];
            
            var region = RegionEndpoint.GetBySystemName(regionName);

            services.AddSingleton<IAmazonSecretsManager>(
                new AmazonSecretsManagerClient(region)
            );

            services.AddScoped<IFileStorageService, AwsS3StorageService>();

            return services;
        }
    }
}