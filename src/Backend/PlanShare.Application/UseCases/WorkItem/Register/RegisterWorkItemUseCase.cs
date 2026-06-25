using AutoMapper;
using FluentValidation.Results;
using PlanShare.Communication.Requests;
using PlanShare.Communication.Responses;
using PlanShare.Domain.Extensions;
using PlanShare.Domain.Repositories;
using PlanShare.Domain.Repositories.WorkItem;
using PlanShare.Exceptions.ExceptionsBase;

namespace PlanShare.Application.UseCases.WorkItem.Register;
public class RegisterWorkItemUseCase(IMapper mapper, IUnitOfWork unitOfWork, IWorkItemWriteOnlyRepository repository)
    : IRegisterWorkItemUseCase
{
    public async Task<ResponseRegisteredWorkItemJson> Execute(RequestRegisterWorkItemJson request)
    {
        await Validate(request: request);

        Domain.Entities.WorkItem? entity = mapper.Map<Domain.Entities.WorkItem>(source: request);

        await repository.Add(workItem: entity);

        await unitOfWork.Commit();

        return new()
        {
            Id = entity.Id,
            Title = entity.Title,
        };
    }

    private async Task Validate(RequestRegisterWorkItemJson request)
    {
        ValidationResult? result = new RegisterWorkItemValidator().Validate(instance: request);

        if (result.IsValid.IsFalse())
        {
            throw new ErrorOnValidationException(listErrors: result.Errors.Select(selector: e => e.ErrorMessage).ToList());
        }
    }
}
