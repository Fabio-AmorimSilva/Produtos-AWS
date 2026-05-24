namespace ProdutosAws.Infrastructure;

public class ProductAwsDbContext(DbContextOptions<ProductAwsDbContext> options) : DbContext(options), IProductAwsDbContext
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}