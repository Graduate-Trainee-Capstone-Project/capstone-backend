using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnboardingPlatform.API.Mappers;
using OnboardingPlatform.Core.Enums;
using OnboardingPlatform.Data.Implementations;

namespace OnboardingPlatform.API.Controllers
{
    [ApiController]
    [Route("/products")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ProductsController(AppDbContext db) => _db = db;

        /// <summary>
        /// GET /api/products — Frontend uses this to build the product picker screen
        /// and dynamically know what identifier to ask for (BVN vs NIN+phone etc.)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _db.Products
                .Where(p => p.IsActive)
                .ToListAsync();

            return Ok(products.Select(ProductMapper.ToResponse));
        }

        /// <summary>
        /// GET /api/products/{productCode}
        /// </summary>
        [HttpGet("by/{productCode}")]
        public async Task<IActionResult> GetByCode(string productCode)
        {
            if (!Enum.TryParse<ProductCode>(
                    productCode,
                    ignoreCase: true,
                    out var parsedProductCode))
            {
                return NotFound();
            }

            var product = await _db.Products
                .FirstOrDefaultAsync(p =>
                    p.ProductCode == parsedProductCode &&
                    p.IsActive);

            return product is null
                ? NotFound()
                : Ok(ProductMapper.ToResponse(product));
        }
    }
}
