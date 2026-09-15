using Microsoft.AspNetCore.Mvc;
using OnboardingPlatform.Services.Interfaces;

namespace OnboardingPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PensionAccountsController : ControllerBase
    {
        private readonly IPensionAccountService _pensionService;

        public PensionAccountsController(IPensionAccountService pensionService)
        {
            _pensionService = pensionService;
        }

        [HttpGet("GetPensionAccountByCustomerProduct/{customerProductId:guid}")]
        public async Task<IActionResult> GetPensionAccountByCustomerProduct(Guid customerProductId)
        {
            var account = await _pensionService.GetByCustomerProductIdAsync(customerProductId);
            return account is null ? NotFound() : Ok(account);
        }
    }
}