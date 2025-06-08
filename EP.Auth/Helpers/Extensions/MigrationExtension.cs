using EP.Auth.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EP.Auth.Helpers.Extensions
{
    /// <summary>
    /// Applies any pending database migrations at application startup to ensure
    /// the database schema is up-to-date with the latest changes.
    /// </summary>
    public static class MigrationExtensions
    {
        public static void ApplyPendingMigrations(this IApplicationBuilder app)
        {
            // apply pending migrations automatically
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                try
                {
                    var dbContext = scope.ServiceProvider.GetService<DataContext>();
                    dbContext.Database.Migrate();
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred during the automatic migration.");
                }
            };
        }
    }
}
