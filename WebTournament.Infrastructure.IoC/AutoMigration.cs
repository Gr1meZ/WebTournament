using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebTournament.Infrastructure.Data;
using WebTournament.Infrastructure.Data.Context;

namespace WebTournament.Infrastructure.IoC
{
    internal static class AutoMigration
    {
        internal static async Task AutoMigrateDatabaseAsync (this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();

            var dbContext = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            await dbContext.Database.MigrateAsync();
        }
    }
}
