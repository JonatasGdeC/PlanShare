using AutoMapper;
using FluentValidation.Results;
using PlanShare.Communication.Requests;
using PlanShare.Domain.Extensions;
using PlanShare.Domain.Repositories;
using PlanShare.Domain.Repositories.WorkItem;
using PlanShare.Domain.Services.LoggedUser;
using PlanShare.Exceptions;
using PlanShare.Exceptions.ExceptionsBase;

namespace PlanShare.Application.UseCases.WorkItem.Update;
public class UpdateWorkItemUseCase(
    ILoggedUser user,
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IWorkItemUpdateOnlyRepository repository)
    : IUpdateWorkItemUseCase
{
    public async Task Execute(Guid workItemId, RequestUpdateWorkItemJson request)
    {
        Validate(request: request);

        Domain.Entities.User loggedUser = await user.Get();

        Domain.Entities.WorkItem? workItem = await repository.GetById(user: loggedUser, id: workItemId);
        if (workItem is null)
            throw new NotFoundException(mensagem: ResourceMessagesException.WORK_ITEM_NOT_FOUND);

        mapper.Map(source: request, destination: workItem);

        repository.Update(workItem: workItem);

        await unitOfWork.Commit();
    }

    private static void Validate(RequestUpdateWorkItemJson request)
    {
        ValidationResult? result = new UpdateWorkItemValidator().Validate(instance: request);

        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(listErrors: result.Errors.Select(selector: e => e.ErrorMessage).ToList());
    }
}