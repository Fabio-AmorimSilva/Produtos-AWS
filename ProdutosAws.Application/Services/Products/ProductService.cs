namespace ProdutosAzure.Application.Services.Products;

public class ProductService(IProductAzureDbContext context) : IProductService
{
    public async Task<IEnumerable<ProductDto>> ListProductsAsync()
    {
        var products = await context.Products
            .Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                ProductCategory = p.Category
            })
            .ToListAsync();

        return products;
    }

    public async Task<Product?> GetProductByIdAsync(Guid productId)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        return product;
    }

    public async Task<Guid> CreateAsync(CreateProductDto dto)
    {
        var product = new Product(
            name: dto.Name,
            category: dto.ProductCategory
        );

        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();

        return product.Id;
    }

    public async Task UpdateAsync(UpdateProductDto dto)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == dto.ProductId);

        if (product is null)
            return;

        product.Update(
            name: dto.Name,
            productCategory: dto.ProductCategory
        );

        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid productId)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        if (product is null)
            return;

        context.Products.Remove(product);
        await context.SaveChangesAsync();
    }
}