using Microsoft.EntityFrameworkCore;
using PlanShare.Domain.Entities;
using PlanShare.Domain.Repositories.WorkItem;

namespace PlanShare.Infrastructure.DataAccess.Repositories;
internal sealed class WorkItemRepository(PlanShareDbContext dbContext)
    : IWorkItemWriteOnlyRepository, IWorkItemReadOnlyRepository, IWorkItemUpdateOnlyRepository
{
    public async Task Add(WorkItem workItem) => await dbContext.WorkItems.AddAsync(entity: workItem);

    public async Task Delete(Guid id)
    {
        WorkItem? workItem = await dbContext.WorkItems.FindAsync(id);

        dbContext.WorkItems.Remove(entity: workItem!);
    }

    async Task<WorkItem?> IWorkItemUpdateOnlyRepository.GetById(User user, Guid id)
    {
        return await dbContext
            .WorkItems
            .SingleOrDefaultAsync(predicate: workItem => workItem.Id == id && workItem.Assignees.Any(assignee => assignee.UserId == user.Id));
    }

    async Task<WorkItem?> IWorkItemReadOnlyRepository.GetById(User user, Guid id)
    {
        return await dbContext
            .WorkItems
            .AsNoTracking()
            .SingleOrDefaultAsync(predicate: workItem => workItem.Id == id && workItem.Assignees.Any(assignee => assignee.UserId == user.Id));
    }

    public void Update(WorkItem workItem) => dbContext.WorkItems.Update(entity: workItem);

    public async Task<List<WorkItem>> GetAll(User user)
    {
        return await dbContext
            .WorkItems
            .AsNoTracking()
            .Where(predicate: workItem => workItem.Assignees.Any(assignee => assignee.UserId == user.Id))
            .ToListAsync();
    }
}
