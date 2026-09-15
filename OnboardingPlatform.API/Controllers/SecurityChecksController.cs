using Microsoft.AspNetCore.Mvc;
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
    }
}