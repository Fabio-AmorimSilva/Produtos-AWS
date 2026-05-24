namespace ProdutosAzure.Application.Common;

public interface IProductAzureDbContext
{
    DbSet<Product> Products { get; }  
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}