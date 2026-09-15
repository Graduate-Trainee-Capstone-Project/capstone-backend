using Microsoft.AspNetCore.Mvc;
using OnboardingPlatform.Services.Interfaces;

namespace OnboardingPlatform.API.Controllers
{
    [ApiController]
    [Route("/currentAccountBy")]
    public class CurrentAccountsController : ControllerBase
    {
        private readonly ICurrentAccountService _currentService;

        public CurrentAccountsController(ICurrentAccountService currentService)
        {
            _currentService = currentService;
        }

       [HttpGet("{customerProductId:guid}")]
        public async Task<IActionResult> GetCurrentAccountByCustomerProduct(Guid customerProductId)
        {
            var account = await _currentService.GetByCustomerProductIdAsync(customerProductId);
            return account is null ? NotFound() : Ok(account);
        }
    }
}