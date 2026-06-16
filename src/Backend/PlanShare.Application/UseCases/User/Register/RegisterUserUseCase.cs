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
public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserReadOnlyRepository _userReadOnlyRepository;
    private readonly IUserWriteOnlyRepository _repository;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly ITokenService _tokenService;

    public RegisterUserUseCase(
        IMapper mapper,
        IUnitOfWork unitOfWork,
        IUserWriteOnlyRepository repository,
        IUserReadOnlyRepository userReadOnlyRepository,
        IPasswordEncripter passwordEncripter,
        ITokenService tokenService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userReadOnlyRepository = userReadOnlyRepository;
        _repository = repository;
        _passwordEncripter = passwordEncripter;
        _tokenService = tokenService;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request: request);

        Domain.Entities.User? user = _mapper.Map<Domain.Entities.User>(source: request);
        user.Password = _passwordEncripter.Encrypt(password: request.Password);

        await _repository.Add(user: user);

        await _unitOfWork.Commit();

        TokensDto tokens = await _tokenService.GenerateTokens(user: user);

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

        bool emailExist = await _userReadOnlyRepository.ExistActiveUserWithEmail(email: request.Email);
        if (emailExist)
            result.Errors.Add(item: new ValidationFailure(propertyName: string.Empty, errorMessage: ResourceMessagesException.EMAIL_ALREADY_REGISTERED));

        if (result.IsValid.IsFalse())
            throw new ErrorOnValidationException(listErrors: result.Errors.Select(selector: e => e.ErrorMessage).ToList());
    }
}