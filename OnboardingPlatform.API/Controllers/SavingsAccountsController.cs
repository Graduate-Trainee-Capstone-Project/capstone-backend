using Microsoft.AspNetCore.Mvc;
using OnboardingPlatform.Services.Interfaces;

namespace OnboardingPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SavingsAccountsController : ControllerBase
    {
        private readonly ISavingsAccountService _savingsService;

        public SavingsAccountsController(ISavingsAccountService savingsService)
        {
            _savingsService = savingsService;
        }

       [HttpGet("GetSavingsAccountByCustomerProduct/{customerProductId:guid}")]
        public async Task<IActionResult> GetSavingsAccountByCustomerProduct(Guid customerProductId)
        {
            var account = await _savingsService.GetByCustomerProductIdAsync(customerProductId);
            return account is null ? NotFound() : Ok(account);
        }
    }
}