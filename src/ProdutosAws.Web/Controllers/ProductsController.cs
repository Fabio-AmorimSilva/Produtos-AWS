namespace ProdutosAws.Web.Controllers;

public class ProductsController(IProductService productService) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var products = await productService.ListProductsAsync();

        return View(products);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateProductDto dto)
    {
        await productService.CreateAsync(dto);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var product = await productService.GetProductByIdAsync(id);

        var dto = new UpdateProductDto
        {
            ProductId = product?.Id ?? Guid.Empty,
            Name = product?.Name ?? string.Empty,
            ProductCategory = product!.Category
        };

        return View(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Update(UpdateProductDto dto)
    {
        await productService.UpdateAsync(dto);

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Upload(Guid id)
    {
        var product = await productService.GetProductByIdAsync(id);

        var dto = new UpdateProductImage
        {
            ProductId = product?.Id ?? Guid.Empty
        };
        
        return View(dto);       
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file, Guid id)
    {
        if (file.Length == 0)
            return BadRequest();

        var stream = new MemoryStream();
        await file.CopyToAsync(stream);
        
        var dto = new UpdateProductImage
        {
            ProductId = id,
            Path = file.FileName,
            Stream = stream
        };

        await productService.UpdateImageAsync(dto);

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetImage(Guid id)
    {
        var product = await productService.GetProductByIdAsync(id);

        if (product is null)
            return NotFound();

        return View(product);
    }

    [HttpGet]
    public async Task<IActionResult> DownloadImage(Guid productId, string path)
    {
        var url = await productService.GetImageUrlAsync(new DownloadImageDto { ProductId = productId, Key = path });

        if (string.IsNullOrEmpty(url))
            return NotFound();

        return Redirect(url);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        await productService.DeleteAsync(id);

        return RedirectToAction(nameof(Index));
    }
}