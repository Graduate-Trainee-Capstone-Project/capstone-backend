using Microsoft.EntityFrameworkCore;
using OnboardingPlatform.Data;
using OnboardingPlatform.Data.Implementations;
using OnboardingPlatform.Services.Implementations;
using OnboardingPlatform.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson();

builder.Services.AddLogging(builder =>
{
    builder.SetMinimumLevel(LogLevel.Information);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Unified Customer Onboarding API", Version = "v1" });
});

// Register DbContexts
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// ── DI ────────────────────────────────────────────────────────
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
    });
});

// Register health service
builder.Services.AddSingleton<IHealthService, HealthService>();

var app = builder.Build();

app.ApplyDatabaseMigrations<AppDbContext>(app.Services.GetRequiredService<ILogger<AppDbContext>>());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Commented for container-friendly testing (remove if you want HTTPS redirects)
// app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();