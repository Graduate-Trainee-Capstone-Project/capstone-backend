using Microsoft.AspNetCore.Mvc;
using OnboardingPlatform.Services.Interfaces;

namespace OnboardingPlatform.API.Controllers;

[ApiController]
[Route("[controller]")]
public class HealthController : ControllerBase
{
    private readonly IHealthService _health;

    public HealthController(IHealthService health)
    {
        _health = health;
    }

    [HttpGet]
    public IActionResult Get() => Ok(new { status = _health.GetStatus() });
}