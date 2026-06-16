using PlanShare.Domain.Repositories;
using PlanShare.Domain.Repositories.WorkItem;
using PlanShare.Domain.Services.LoggedUser;
using PlanShare.Exceptions;
using PlanShare.Exceptions.ExceptionsBase;

namespace PlanShare.Application.UseCases.WorkItem.Delete;
public class DeleteWorkItemUseCase(
    ILoggedUser user,
    IWorkItemReadOnlyRepository repositoryRead,
    IWorkItemWriteOnlyRepository repositoryWrite,
    IUnitOfWork unitOfWork)
    : IDeleteWorkItemUseCase
{
    public async Task Execute(Guid workItemId)
    {
        Domain.Entities.User loggedUser = await user.Get();

        Domain.Entities.WorkItem? workItem = await repositoryRead.GetById(user: loggedUser, id: workItemId);

        if(workItem is null)
            throw new NotFoundException(mensagem: ResourceMessagesException.WORK_ITEM_NOT_FOUND);

        await repositoryWrite.Delete(id: workItemId);

        await unitOfWork.Commit();
    }
}
