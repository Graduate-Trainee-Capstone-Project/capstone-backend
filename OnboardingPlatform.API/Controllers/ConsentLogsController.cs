using Microsoft.AspNetCore.Mvc;
using OnboardingPlatform.Core.DTOs.Requests;
using OnboardingPlatform.Core.Enums;
using OnboardingPlatform.Services.Interfaces;

namespace OnboardingPlatform.API.Controllers
{
    [ApiController]
    [Route("/consentBy")]
    public class ConsentLogsController : ControllerBase
    {
        private readonly IConsentService _consentService;

        public ConsentLogsController(IConsentService consentService)
        {
            _consentService = consentService;
        }

        [HttpGet("/{customerId:guid}")]
        public async Task<IActionResult> GetConsentsByCustomer(Guid customerId)
            => Ok(await _consentService.GetByCustomerAsync(customerId));


        [HttpPost("/{customerId:guid}")]
        public async Task<IActionResult> RecordConsent(Guid customerId, [FromBody] ConsentRequest request)
        {
            if (!Enum.TryParse<Channel>(request.Channel, true, out var channel))
                return BadRequest(new { message = $"Unknown channel '{request.Channel}'." });

            try
            {
                var result = await _consentService.RecordConsentAsync(customerId, request.ProductId, channel);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }
    }
}