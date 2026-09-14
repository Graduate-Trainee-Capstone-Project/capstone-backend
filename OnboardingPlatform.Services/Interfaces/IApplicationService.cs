using OnboardingPlatform.Core.DTOs.Requests;
using OnboardingPlatform.Core.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnboardingPlatform.Services.Interfaces
{
    public interface IApplicationService
    {
        Task<StartApplicationResponse> StartApplicationAsync(StartApplicationRequest request);
        Task<FinalizeApplicationResponse> FinalizeApplicationAsync(FinalizeApplicationRequest request);
    }
}
