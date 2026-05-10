using ProdutosAws.Application.Services.Products;

namespace ProdutosAws.Application;

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