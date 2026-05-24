namespace ProdutosAzure.Application;

public static class DependencyInjection
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplication()
        {
            services.AddScoped<IProductService, ProductService>();
            
            return services;
        }
    }
}