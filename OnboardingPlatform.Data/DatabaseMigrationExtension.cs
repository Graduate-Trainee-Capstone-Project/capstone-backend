using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace OnboardingPlatform.Data
{
    public static class DatabaseMigrationExtension
    {

        public static IApplicationBuilder ApplyDatabaseMigrations<TContext>(
           this IApplicationBuilder app,
           ILogger<TContext> logger) where TContext : DbContext
        {
            try
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var context = scope.ServiceProvider.GetRequiredService<TContext>();

                    logger.LogInformation("Checking for pending migrations...");
                    var pendingMigrations = context.Database.GetPendingMigrations();

                    if (pendingMigrations.Any())
                    {
                        logger.LogInformation("Found {count} pending migrations. Applying...", pendingMigrations.Count());
                        context.Database.Migrate();
                        logger.LogInformation("Successfully applied all pending migrations");
                    }
                    else
                    {
                        logger.LogInformation("No pending migrations found");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while applying migrations");
                throw;
            }

            return app;
        }
    }
}
