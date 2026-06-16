using AutoMapper;
using FluentValidation.Results;
using PlanShare.Communication.Requests;
using PlanShare.Communication.Responses;
using PlanShare.Domain.Extensions;
using PlanShare.Domain.Repositories;
using PlanShare.Domain.Repositories.WorkItem;
using PlanShare.Exceptions.ExceptionsBase;

namespace PlanShare.Application.UseCases.WorkItem.Register;
public class RegisterWorkItemUseCase : IRegisterWorkItemUseCase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWorkItemWriteOnlyRepository _repository;

    public RegisterWorkItemUseCase(IMapper mapper, IUnitOfWork unitOfWork, IWorkItemWriteOnlyRepository repository)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _repository = repository;
    }

    public async Task<ResponseRegisteredWorkItemJson> Execute(RequestRegisterWorkItemJson request)
    {
        await Validate(request: request);

        Domain.Entities.WorkItem? entity = _mapper.Map<Domain.Entities.WorkItem>(source: request);

        await _repository.Add(workItem: entity);

        await _unitOfWork.Commit();

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
            throw new ErrorOnValidationException(listErrors: result.Errors.Select(selector: e => e.ErrorMessage).ToList());
    }
}
