using FluentValidation.Results;
using PlanShare.Communication.Requests;
using PlanShare.Domain.Extensions;
using PlanShare.Domain.Repositories;
using PlanShare.Domain.Repositories.User;
using PlanShare.Domain.Services.LoggedUser;
using PlanShare.Exceptions;
using PlanShare.Exceptions.ExceptionsBase;

namespace PlanShare.Application.UseCases.User.Update;
public class UpdateUserUseCase(
    ILoggedUser loggedUser,
    IUserUpdateOnlyRepository repository,
    IUserReadOnlyRepository userReadOnlyRepository,
    IUnitOfWork unitOfWork)
    : IUpdateUserUseCase
{
    public async Task Execute(RequestUpdateUserJson request)
    {
        Domain.Entities.User loggedUser1 = await loggedUser.Get();

        await Validate(request: request, currentEmail: loggedUser1.Email);

        Domain.Entities.User user = await repository.GetById(id: loggedUser1.Id);

        user.Name = request.Name;
        user.Email = request.Email;

        repository.Update(user: user);

        await unitOfWork.Commit();
    }

    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        UpdateUserValidator validator = new UpdateUserValidator();

        ValidationResult? result = validator.Validate(instance: request);

        if (currentEmail.Equals(value: request.Email).IsFalse())
        {
            bool userExist = await userReadOnlyRepository.ExistActiveUserWithEmail(email: request.Email);
            if (userExist)
            {
                result.Errors.Add(item: new ValidationFailure(propertyName: string.Empty, errorMessage: ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
            }
        }

        if (result.IsValid.IsFalse())
        {
            List<string> errorMessages = result.Errors.Select(selector: error => error.ErrorMessage).ToList();

            throw new ErrorOnValidationException(listErrors: errorMessages);
        }
    }
}