using Microsoft.AspNetCore.Mvc;
using OnboardingPlatform.Core.DTOs.Requests;
using OnboardingPlatform.Services.Interfaces;

namespace OnboardingPlatform.API.Controllers
{
    [ApiController]
    [Route("/applications")]
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationService _appService;

        public ApplicationsController(IApplicationService appService)
            => _appService = appService;

        /// <summary>
        /// POST /api/applications/start
        /// The CORE endpoint — product + identifier in, DraftId + routing decision out.
        /// </summary>
        [HttpPost("start")]
        public async Task<IActionResult> Start([FromBody] StartApplicationRequest request)
        {
            try
            {
                var result = await _appService.StartApplicationAsync(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// POST /api/applications/{draftId}/finalize
        /// Converts a completed draft into a real Customer + CustomerProduct.
        /// </summary>
        [HttpPost("{draftId}/finalize")]
        public async Task<IActionResult> Finalize(Guid draftId)
        {
            try
            {
                var result = await _appService.FinalizeApplicationAsync(draftId);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
