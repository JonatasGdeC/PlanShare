using PlanShare.Application.Services.Authentication;
using PlanShare.Communication.Requests;
using PlanShare.Communication.Responses;
using PlanShare.Domain.Dtos;
using PlanShare.Domain.Repositories.User;
using PlanShare.Domain.Security.Cryptography;
using PlanShare.Exceptions.ExceptionsBase;

namespace PlanShare.Application.UseCases.Login.DoLogin;
public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _repository;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly ITokenService _tokenService;

    public DoLoginUseCase(
        IUserReadOnlyRepository repository,
        IPasswordEncripter passwordEncripter,
        ITokenService tokenService)
    {
        _passwordEncripter = passwordEncripter;
        _repository = repository;
        _tokenService = tokenService;
    }

    public async Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request)
    {
        Domain.Entities.User? user = await _repository.GetUserByEmail(email: request.Email);

        if (user is null)
            throw new InvalidLoginException();

        bool passwordMatch = _passwordEncripter.IsValid(password: request.Password, passwordHash: user.Password);

        if (passwordMatch == false)
            throw new InvalidLoginException();

        TokensDto tokens = await _tokenService.GenerateTokens(user: user);

        return new ResponseRegisteredUserJson
        {
            Id = user.Id,
            Name = user.Name,
            Tokens = new ResponseTokensJson
            {
                AccessToken = tokens.Access
            }
        };
    }
}