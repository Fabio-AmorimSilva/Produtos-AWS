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
            services.AddDbContext<ProductsAwsDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );

            services.AddScoped<IProductAwsDbContext>(provider => provider.GetRequiredService<ProductsAwsDbContext>());

            return services;
        }

        private IServiceCollection AddAwsS3(IConfiguration configuration)
        {
            Console.WriteLine("===== AWS CONFIG =====");

            Console.WriteLine($"AWS:Region = {configuration["AWS:Region"]}");
            Console.WriteLine($"AWS__Region = {configuration["AWS__Region"]}");

            Console.WriteLine($"AWS:BucketName = {configuration["AWS:BucketName"]}");
            Console.WriteLine($"AWS__BucketName = {configuration["AWS__BucketName"]}");

            services.Configure<AwsSettings>(
                configuration.GetSection("AWS")
            );

            var regionName = configuration["AWS:Region"];

            if (string.IsNullOrWhiteSpace(regionName))
                throw new Exception("AWS:Region não encontrada.");

            var region = RegionEndpoint.GetBySystemName(regionName);

            services.AddSingleton<IAmazonS3>(
                new AmazonS3Client(region)
            );

            services.AddScoped<IFileStorageService, AwsS3StorageService>();

            return services;
        }
    }
}