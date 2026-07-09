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
            try
            {
                Console.WriteLine("Criando cliente Secrets Manager...");

                using var client = new AmazonSecretsManagerClient(RegionEndpoint.SAEast1);

                Console.WriteLine("Buscando secret...");

                var response = client
                    .GetSecretValueAsync(new GetSecretValueRequest
                    {
                        SecretId = "ProdutosAws/Database"
                    })
                    .GetAwaiter()
                    .GetResult();

                Console.WriteLine("Secret recuperado.");

                Console.WriteLine(response.SecretString);

                var secrets = JsonDocument.Parse(response.SecretString);

                Console.WriteLine("JSON convertido.");

                var connectionString = secrets.RootElement
                    .GetProperty("ConnectionString")
                    .GetString();

                Console.WriteLine("ConnectionString encontrada.");

                services.AddDbContext<ProductsAwsDbContext>(options =>
                    options.UseSqlServer(connectionString));

                return services;
            }
            catch (Exception ex)
            {
                Console.WriteLine("======================================");
                Console.WriteLine(ex.ToString());
                Console.WriteLine("======================================");

                throw;
            }
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