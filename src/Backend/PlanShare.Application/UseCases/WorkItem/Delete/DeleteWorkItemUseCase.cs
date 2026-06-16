using PlanShare.Domain.Repositories;
using PlanShare.Domain.Repositories.WorkItem;
using PlanShare.Domain.Services.LoggedUser;
using PlanShare.Exceptions;
using PlanShare.Exceptions.ExceptionsBase;

namespace PlanShare.Application.UseCases.WorkItem.Delete;
public class DeleteWorkItemUseCase : IDeleteWorkItemUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IWorkItemReadOnlyRepository _repositoryRead;
    private readonly IWorkItemWriteOnlyRepository _repositoryWrite;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteWorkItemUseCase(
        ILoggedUser loggedUser,
        IWorkItemReadOnlyRepository repositoryRead,
        IWorkItemWriteOnlyRepository repositoryWrite,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _repositoryRead = repositoryRead;
        _repositoryWrite = repositoryWrite;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(Guid workItemId)
    {
        Domain.Entities.User loggedUser = await _loggedUser.Get();

        Domain.Entities.WorkItem? workItem = await _repositoryRead.GetById(user: loggedUser, id: workItemId);

        if(workItem is null)
            throw new NotFoundException(mensagem: ResourceMessagesException.WORK_ITEM_NOT_FOUND);

        await _repositoryWrite.Delete(id: workItemId);

        await _unitOfWork.Commit();
    }
}
