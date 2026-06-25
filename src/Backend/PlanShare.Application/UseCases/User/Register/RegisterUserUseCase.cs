using AutoMapper;
using FluentValidation.Results;
using PlanShare.Application.Services.Authentication;
using PlanShare.Communication.Requests;
using PlanShare.Communication.Responses;
using PlanShare.Domain.Dtos;
using PlanShare.Domain.Extensions;
using PlanShare.Domain.Repositories;
using PlanShare.Domain.Repositories.User;
using PlanShare.Domain.Security.Cryptography;
using PlanShare.Exceptions;
using PlanShare.Exceptions.ExceptionsBase;

namespace PlanShare.Application.UseCases.User.Register;
public class RegisterUserUseCase(
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IUserWriteOnlyRepository repository,
    IUserReadOnlyRepository userReadOnlyRepository,
    IPasswordEncripter passwordEncripter,
    ITokenService tokenService)
    : IRegisterUserUseCase
{
    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request: request);

        Domain.Entities.User? user = mapper.Map<Domain.Entities.User>(source: request);
        user.Password = passwordEncripter.Encrypt(password: request.Password);

        await repository.Add(user: user);

        await unitOfWork.Commit();

        TokensDto tokens = await tokenService.GenerateTokens(user: user);

        return new()
        {
            Id = user.Id,
            Name = user.Name,
            Tokens = new()
            {
                AccessToken = tokens.Access
            }
        };
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        ValidationResult? result = new RegisterUserValidator().Validate(instance: request);

        bool emailExist = await userReadOnlyRepository.ExistActiveUserWithEmail(email: request.Email);
        if (emailExist)
        {
            result.Errors.Add(item: new ValidationFailure(propertyName: string.Empty, errorMessage: ResourceMessagesException.EMAIL_ALREADY_REGISTERED));
        }

        if (result.IsValid.IsFalse())
        {
            throw new ErrorOnValidationException(listErrors: result.Errors.Select(selector: e => e.ErrorMessage).ToList());
        }
    }
}