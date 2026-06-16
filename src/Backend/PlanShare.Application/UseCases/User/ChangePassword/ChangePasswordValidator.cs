using FluentValidation;
using PlanShare.Application.SharedValidators;
using PlanShare.Communication.Requests;

namespace PlanShare.Application.UseCases.User.ChangePassword;
public class ChangePasswordValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangePasswordValidator()
    {
        RuleFor(expression: x => x.NewPassword).SetValidator(validator: new PasswordValidator<RequestChangePasswordJson>());
    }
}