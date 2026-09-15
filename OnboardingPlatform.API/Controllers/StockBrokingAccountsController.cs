using Microsoft.AspNetCore.Mvc;
using OnboardingPlatform.Services.Interfaces;

namespace OnboardingPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StockBrokingAccountsController : ControllerBase
    {
        private readonly IStockBrokingAccountService _stockBrokingService;

        public StockBrokingAccountsController(IStockBrokingAccountService stockBrokingService)
        {
            _stockBrokingService = stockBrokingService;
        }

        [HttpGet("GetStockBrokingAccountByCustomerProduct/{customerProductId:guid}")]
        public async Task<IActionResult> GetStockBrokingAccountByCustomerProduct(Guid customerProductId)
        {
            var account = await _stockBrokingService.GetByCustomerProductIdAsync(customerProductId);
            return account is null
                ? NotFound(new { message = $"No stock broking account found for customer product '{customerProductId}'." })
                : Ok(account);
        }
    }
}