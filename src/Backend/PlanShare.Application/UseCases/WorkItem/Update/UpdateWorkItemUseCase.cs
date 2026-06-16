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
public class UpdateWorkItemUseCase : IUpdateWorkItemUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWorkItemUpdateOnlyRepository _repository;

    public UpdateWorkItemUseCase(
        ILoggedUser loggedUser,
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IWorkItemUpdateOnlyRepository repository)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _repository = repository;
        _loggedUser = loggedUser;
    }

    public async Task Execute(Guid workItemId, RequestUpdateWorkItemJson request)
    {
        Validate(request: request);

        Domain.Entities.User loggedUser = await _loggedUser.Get();

        Domain.Entities.WorkItem? workItem = await _repository.GetById(user: loggedUser, id: workItemId);
        if (workItem is null)
            throw new NotFoundException(mensagem: ResourceMessagesException.WORK_ITEM_NOT_FOUND);

        _mapper.Map(source: request, destination: workItem);

        _repository.Update(workItem: workItem);

        await _unitOfWork.Commit();
    }

    private static void Validate(RequestUpdateWorkItemJson request)
    {
        ValidationResult? result = new UpdateWorkItemValidator().Validate(instance: request);

        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(listErrors: result.Errors.Select(selector: e => e.ErrorMessage).ToList());
    }
}