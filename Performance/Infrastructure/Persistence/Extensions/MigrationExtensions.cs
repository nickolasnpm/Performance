using Microsoft.EntityFrameworkCore;

namespace Performance.Infrastructure.Persistence.Extensions
{
    public static class MigrationExtensions
    {
        public static async Task ApplyMigrations(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();

            try
            {
                await context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<UserDbContext>>();
                logger.LogError(ex, "Database Migration Failed on Raspberry Pi.");
            }
        }
    }
}
