using OnboardingPlatform.Core.DTOs.Requests;
using OnboardingPlatform.Core.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingPlatform.Services.Interfaces
{
    public interface ISecurityService
    {
        Task<SecurityCheckResponse> PerformCheckAsync(Guid draftId, SecurityCheckRequest request);
        Task<List<SecurityCheckResponse>> GetChecksForDraftAsync(Guid draftId);
    }
}
