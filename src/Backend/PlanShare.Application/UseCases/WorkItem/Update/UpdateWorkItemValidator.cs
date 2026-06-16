using FluentValidation;
using PlanShare.Communication.Requests;
using PlanShare.Exceptions;

namespace PlanShare.Application.UseCases.WorkItem.Update;
public class UpdateWorkItemValidator : AbstractValidator<RequestUpdateWorkItemJson>
{
    public UpdateWorkItemValidator()
    {
        RuleFor(expression: request => request.Title).NotEmpty().WithMessage(errorMessage: ResourceMessagesException.NAME_EMPTY);
        RuleFor(expression: request => request.DueDate.Date).GreaterThanOrEqualTo(valueToCompare: DateTime.UtcNow.Date);
    }
}