using Microsoft.AspNetCore.Mvc;
using OnboardingPlatform.Core.DTOs.Requests;
using OnboardingPlatform.Services.Interfaces;

namespace OnboardingPlatform.API.Controllers
{
    [ApiController]
    [Route("/securityCheckBy")]
    public class SecurityChecksController : ControllerBase
    {
        private readonly ISecurityService _securityService;

        public SecurityChecksController(ISecurityService securityService)
        {
            _securityService = securityService;
        }
                
        [HttpGet("{draftId:guid}")]
        public async Task<IActionResult> GetSecurityChecksByDraft(Guid draftId)
            => Ok(await _securityService.GetChecksForDraftAsync(draftId));

        [HttpPost("{draftId:guid}")]
        public async Task<IActionResult> PerformSecurityCheck(Guid draftId, [FromBody] SecurityCheckRequest request)
        {
            try
            {
                var result = await _securityService.PerformCheckAsync(draftId, request);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}