using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PlanShare.Infrastructure.DataAccess.Migrations;

public static class DataBaseMigration
{
    public static async Task MigrateDatabase(IServiceProvider serviceProvider)
    {
        using PlanShareDbContext dbContext = serviceProvider.GetRequiredService<PlanShareDbContext>();
        await dbContext.Database.MigrateAsync();
    }
}