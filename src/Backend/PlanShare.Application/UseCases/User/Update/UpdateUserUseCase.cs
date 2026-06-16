using FluentValidation.Results;
using PlanShare.Communication.Requests;
using PlanShare.Domain.Extensions;
using PlanShare.Domain.Repositories;
using PlanShare.Domain.Repositories.User;
using PlanShare.Domain.Services.LoggedUser;
using PlanShare.Exceptions;
using PlanShare.Exceptions.ExceptionsBase;

namespace PlanShare.Application.UseCases.User.Update;
public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateUserUseCase(
        ILoggedUser loggedUser,
        IUserUpdateOnlyRepository repository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _userReadOnlyRepository = userReadOnlyRepository;
    }

    public async Task Execute(RequestUpdateUserJson request)
    {
        Domain.Entities.User loggedUser = await _loggedUser.Get();

        await Validate(request: request, currentEmail: loggedUser.Email);

        Domain.Entities.User user = await _repository.GetById(id: loggedUser.Id);

        user.Name = request.Name;
        user.Email = request.Email;

        _repository.Update(user: user);

        await _unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        UpdateUserValidator validator = new UpdateUserValidator();

        ValidationResult? result = validator.Validate(instance: request);

        if (currentEmail.Equals(value: request.Email).IsFalse())
        {
            bool userExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(email: request.Email);
            if (userExist)
                result.Errors.Add(item: new ValidationFailure(propertyName: string.Empty, errorMessage: ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
        }

        if (result.IsValid.IsFalse())
        {
            List<string> errorMessages = result.Errors.Select(selector: error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(listErrors: errorMessages);
        }
    }
}