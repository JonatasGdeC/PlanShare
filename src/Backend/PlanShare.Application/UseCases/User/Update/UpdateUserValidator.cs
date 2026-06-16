using FluentValidation;
using PlanShare.Communication.Requests;
using PlanShare.Exceptions;

namespace PlanShare.Application.UseCases.User.Update;
public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(expression: user => user.Name).NotEmpty().WithMessage(errorMessage: ResourceMessagesException.NAME_EMPTY);
        RuleFor(expression: user => user.Email)
            .NotEmpty()
            .WithMessage(errorMessage: ResourceMessagesException.EMAIL_EMPTY)
            .EmailAddress()
            .When(predicate: user => string.IsNullOrWhiteSpace(value: user.Email) == false, applyConditionTo: ApplyConditionTo.CurrentValidator)
            .WithMessage(errorMessage: ResourceMessagesException.EMAIL_INVALID);
    }
}