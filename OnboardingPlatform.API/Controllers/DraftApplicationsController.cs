using Microsoft.AspNetCore.Mvc;
using OnboardingPlatform.Core.DTOs.Requests;
using OnboardingPlatform.Core.DTOs.Responses;
using OnboardingPlatform.Services.Interfaces;

namespace OnboardingPlatform.API.Controllers
{
    [ApiController]
    [Route("/applications")]
    public class DraftApplicationsController : ControllerBase
    {
        private readonly IDraftService _draftService;
        private readonly IWebHostEnvironment _env;

        public DraftApplicationsController(IDraftService draftService, IWebHostEnvironment env)
        {
            _draftService = draftService;
            _env = env;
        }

        [HttpPut("{draftId:guid}/save")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> SaveDraft(Guid draftId, [FromForm] SaveDraftRequest request)
        {
            try
            {
                // Build DraftFormData from flat fields
                var formData = new DraftFormData
                {
                    FirstName = request.FirstName,
                    MiddleName = request.MiddleName,
                    LastName = request.LastName,
                    DateOfBirth = request.DateOfBirth,
                    Gender = request.Gender,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    AccountType = request.AccountType,
                    InitialDeposit = request.InitialDeposit,
                    Currency = request.Currency,
                    PreferredBranch = request.PreferredBranch,
                    CheckBookRequested = request.CheckBookRequested
                };

                if (!string.IsNullOrWhiteSpace(request.Street))
                {
                    formData.Address = new List<AddressInfo>
            {
                new AddressInfo
                {
                    HouseNumber = request.HouseNumber,
                    Street = request.Street,
                    City = request.City,
                    State = request.State,
                    Country = request.Country
                }
            };
                }

                if (request.DocumentFile != null && request.DocumentFile.Length > 0)
                {
                    var allowedTypes = new[] { "image/jpeg", "image/png", "image/jpg", "application/pdf" };
                    if (!allowedTypes.Contains(request.DocumentFile.ContentType.ToLower()))
                        return BadRequest(new { message = "Only JPG, PNG, and PDF files are allowed." });

                    var uploadsFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", "uploads");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(request.DocumentFile.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await request.DocumentFile.CopyToAsync(stream);

                    var fileUrl = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";

                    formData.Documents = new List<DocumentInfo>
            {
                new DocumentInfo
                {
                    Type = request.DocumentType ?? "UNKNOWN",
                    Url = fileUrl
                }
            };
                }

                // Build internal request with DraftFormData
                var internalRequest = new InternalSaveDraftRequest
                {
                    CurrentStep = request.CurrentStep,
                    Channel = request.Channel,
                    FormData = formData
                };

                var result = await _draftService.SaveAsync(draftId, internalRequest);
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