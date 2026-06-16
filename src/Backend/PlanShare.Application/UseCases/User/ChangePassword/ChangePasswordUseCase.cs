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
public class ChangePasswordUseCase : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncripter _passwordEncripter;

    public ChangePasswordUseCase(
        ILoggedUser loggedUser,
        IPasswordEncripter passwordEncripter,
        IUserUpdateOnlyRepository repository,
        IUnitOfWork unitOfWork)
    {
        _loggedUser = loggedUser;
        _repository = repository;
        _unitOfWork = unitOfWork;
        _passwordEncripter = passwordEncripter;
    }

    public async Task Execute(RequestChangePasswordJson request)
    {
        Domain.Entities.User loggedUser = await _loggedUser.Get();

        Validate(request: request, loggedUser: loggedUser);

        Domain.Entities.User user = await _repository.GetById(id: loggedUser.Id);
        user.Password = _passwordEncripter.Encrypt(password: request.NewPassword);

        _repository.Update(user: user);

        await _unitOfWork.Commit();
    }

    private void Validate(RequestChangePasswordJson request, Domain.Entities.User loggedUser)
    {
        ChangePasswordValidator validator = new ChangePasswordValidator();

        ValidationResult? result = validator.Validate(instance: request);

        bool passwordMatch = _passwordEncripter.IsValid(password: request.Password, passwordHash: loggedUser.Password);

        if (passwordMatch.IsFalse())
            result.Errors.Add(item: new ValidationFailure(propertyName: string.Empty, errorMessage: ResourceMessagesException.PASSWORD_DIFFERENT_CURRENT_PASSWORD));

        if (result.IsValid.IsFalse())
        {
            List<string> errors = result.Errors.Select(selector: e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(listErrors: errors);
        }
    }
}