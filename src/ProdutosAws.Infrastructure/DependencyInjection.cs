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
            services.Configure<AwsSettings>(configuration.GetSection("AWS"));
            
            var region = RegionEndpoint.GetBySystemName(configuration["AWS:Region"]);

            services.AddSingleton<IAmazonS3>(new AmazonS3Client(region));
            services.AddScoped<IFileStorageService, AwsS3StorageService>();
            
            return services;
        }
    }
}