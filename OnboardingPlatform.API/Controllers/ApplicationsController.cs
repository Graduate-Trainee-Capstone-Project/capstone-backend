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
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "An error occurred while finalizing the application.",
                    details = ex.InnerException?.Message ?? ex.Message
                });
            }
        }
    }
}
