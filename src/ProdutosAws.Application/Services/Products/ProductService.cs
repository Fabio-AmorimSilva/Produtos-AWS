namespace ProdutosAws.Application.Services.Products;

public class ProductService(
    IProductAwsDbContext context,
    IFileStorageService fileStorageService
) : IProductService
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
        var product = await context.Products
            .FirstOrDefaultAsync(p => p.Id == productId);

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

    public async Task<string> UpdateImageAsync(UpdateProductImage dto)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.Id == dto.ProductId);
        
        if (product is null)
            return string.Empty;
        
        var image = new Image(dto.Path);
        
        product.AddImage(image);
        
        var response = await fileStorageService.UploadAsync(image.Path, dto.Stream);
        
        await context.SaveChangesAsync();       
        
        return response;
    }

    public async Task<string> GetImageUrlAsync(DownloadImageDto dto)
    {
        if (string.IsNullOrEmpty(dto.Key))
            return string.Empty;

        var exists = await context.Products.AnyAsync(p => p.Id == dto.ProductId);

        return !exists ? string.Empty : fileStorageService.GenerateUrl(dto.Key);
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