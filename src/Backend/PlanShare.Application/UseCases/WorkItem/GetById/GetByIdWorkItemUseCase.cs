using AutoMapper;
using PlanShare.Communication.Responses;
using PlanShare.Domain.Repositories.WorkItem;
using PlanShare.Domain.Services.LoggedUser;
using PlanShare.Exceptions;
using PlanShare.Exceptions.ExceptionsBase;

namespace PlanShare.Application.UseCases.WorkItem.GetById;
public class GetByIdWorkItemUseCase(
    ILoggedUser user,
    IMapper mapper,
    IWorkItemReadOnlyRepository repository)
    : IGetByIdWorkItemUseCase
{
    public async Task<ResponseWorkItemJson> Execute(Guid workItemId)
    {
        Domain.Entities.User loggedUser = await user.Get();

        Domain.Entities.WorkItem? workItem = await repository.GetById(user: loggedUser, id: workItemId);
        if (workItem is null)
        {
            throw new NotFoundException(mensagem: ResourceMessagesException.WORK_ITEM_NOT_FOUND);
        }

        return mapper.Map<ResponseWorkItemJson>(source: workItem);
    }
}