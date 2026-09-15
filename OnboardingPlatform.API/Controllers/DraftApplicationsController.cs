using Microsoft.AspNetCore.Mvc;
using OnboardingPlatform.Core.DTOs.Requests;
using OnboardingPlatform.Services.Interfaces;

namespace OnboardingPlatform.API.Controllers
{
    [ApiController]
    [Route("/applications")]
    public class DraftApplicationsController : ControllerBase
    {
        private readonly IDraftService _draftService;

        public DraftApplicationsController(IDraftService draftService)
        {
            _draftService = draftService;
        }

        [HttpPut("{draftId:guid}/save")]
        public async Task<IActionResult> SaveDraft(Guid draftId, [FromBody] SaveDraftRequest request)
        {
            try
            {
                var result = await _draftService.SaveAsync(draftId, request);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{draftId:guid}")]
        public async Task<IActionResult> GetDraftById(Guid draftId)
        {
            var draft = await _draftService.GetByIdAsync(draftId);
            if (draft is null)
                return NotFound(new { message = $"Draft '{draftId}' was not found." });

            return Ok(draft);
        }
    }
}