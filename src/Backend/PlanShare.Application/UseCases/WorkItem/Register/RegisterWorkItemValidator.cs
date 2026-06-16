using FluentValidation;
using PlanShare.Communication.Requests;
using PlanShare.Exceptions;

namespace PlanShare.Application.UseCases.WorkItem.Register;
public class RegisterWorkItemValidator : AbstractValidator<RequestRegisterWorkItemJson>
{
    public RegisterWorkItemValidator()
    {
        RuleFor(expression: request => request.Title).NotEmpty().WithMessage(errorMessage: ResourceMessagesException.NAME_EMPTY);
        RuleFor(expression: request => request.DueDate.Date).GreaterThanOrEqualTo(valueToCompare: DateTime.UtcNow.Date);
        RuleFor(expression: request => request.Assignees.Count).GreaterThan(valueToCompare: 0);
    }
}
