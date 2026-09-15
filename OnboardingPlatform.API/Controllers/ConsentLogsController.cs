using Microsoft.AspNetCore.Mvc;
using OnboardingPlatform.Services.Interfaces;

namespace OnboardingPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConsentLogsController : ControllerBase
    {
        private readonly IConsentService _consentService;

        public ConsentLogsController(IConsentService consentService)
        {
            _consentService = consentService;
        }

        [HttpGet("GetConsentsByCustomer/{customerId:guid}")]
        public async Task<IActionResult> GetConsentsByCustomer(Guid customerId)
            => Ok(await _consentService.GetByCustomerAsync(customerId));
    }
}