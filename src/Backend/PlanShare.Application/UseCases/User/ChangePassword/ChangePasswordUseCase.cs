using FluentValidation.Results;
using PlanShare.Communication.Requests;
using PlanShare.Domain.Extensions;
using PlanShare.Domain.Repositories;
using PlanShare.Domain.Repositories.User;
using PlanShare.Domain.Security.Cryptography;
using PlanShare.Domain.Services.LoggedUser;
using PlanShare.Exceptions;
using PlanShare.Exceptions.ExceptionsBase;

namespace PlanShare.Application.UseCases.User.ChangePassword;
public class ChangePasswordUseCase(
    ILoggedUser loggedUser,
    IPasswordEncripter passwordEncripter,
    IUserUpdateOnlyRepository repository,
    IUnitOfWork unitOfWork)
    : IChangePasswordUseCase
{
    public async Task Execute(RequestChangePasswordJson request)
    {
        Domain.Entities.User loggedUser1 = await loggedUser.Get();

        Validate(request: request, loggedUser: loggedUser1);

        Domain.Entities.User user = await repository.GetById(id: loggedUser1.Id);
        user.Password = passwordEncripter.Encrypt(password: request.NewPassword);

        repository.Update(user: user);

        await unitOfWork.Commit();
    }

    private void Validate(RequestChangePasswordJson request, Domain.Entities.User loggedUser)
    {
        ChangePasswordValidator validator = new ChangePasswordValidator();

        ValidationResult? result = validator.Validate(instance: request);

        bool passwordMatch = passwordEncripter.IsValid(password: request.Password, passwordHash: loggedUser.Password);

        if (passwordMatch.IsFalse())
        {
            result.Errors.Add(item: new ValidationFailure(propertyName: string.Empty, errorMessage: ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
        }

        if (result.IsValid.IsFalse())
        {
            List<string> errors = result.Errors.Select(selector: e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(listErrors: errors);
        }
    }
}