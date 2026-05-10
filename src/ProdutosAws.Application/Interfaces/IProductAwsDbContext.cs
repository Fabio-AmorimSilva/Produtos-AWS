namespace ProdutosAws.Application.Interfaces;

public interface IProductAwsDbContext
{
    DbSet<Product> Products { get; }  
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}