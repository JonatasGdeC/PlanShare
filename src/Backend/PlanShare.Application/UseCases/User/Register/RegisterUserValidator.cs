using FluentValidation;
using PlanShare.Application.SharedValidators;
using PlanShare.Communication.Requests;
using PlanShare.Domain.Extensions;
using PlanShare.Exceptions;

namespace PlanShare.Application.UseCases.User.Register;
public class RegisterUserValidator : AbstractValidator<RequestRegisterUserJson>
{
    public RegisterUserValidator()
    {
        RuleFor(expression: request => request.Name).NotEmpty().WithMessage(errorMessage: ResourceMessagesException.NAME_EMPTY);
        RuleFor(expression: request => request.Email).NotEmpty().WithMessage(errorMessage: ResourceMessagesException.EMAIL_EMPTY);
        RuleFor(expression: request => request.Password).SetValidator(validator: new PasswordValidator<RequestRegisterUserJson>());
        When(predicate: request => request.Email.NotEmpty(), action: () =>
        {
            RuleFor(expression: request => request.Email).EmailAddress().WithMessage(errorMessage: ResourceMessagesException.EMAIL_INVALID);
        });
    }
}