using PlanShare.Domain.Repositories;

namespace PlanShare.Infrastructure.DataAccess;
internal sealed class UnitOfWork(PlanShareDbContext dbContext) : IUnitOfWork
{
    public async Task Commit() => await dbContext.SaveChangesAsync();
}
