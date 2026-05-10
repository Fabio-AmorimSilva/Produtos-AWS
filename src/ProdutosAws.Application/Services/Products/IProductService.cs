namespace ProdutosAws.Application.Services.Products;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> ListProductsAsync();
    Task<Product?> GetProductByIdAsync(Guid productId);
    Task<Guid> CreateAsync(CreateProductDto dto);
    Task UpdateAsync(UpdateProductDto dto);
    Task<string> UpdateImageAsync(UpdateProductImage dto);
    Task<string> GetImageUrlAsync(DownloadImageDto dto);
    Task DeleteAsync(Guid productId);
}