using PlanShare.Domain.Dtos;
using PlanShare.Domain.Entities;
using PlanShare.Domain.Repositories;
using PlanShare.Domain.Security.Tokens;

namespace PlanShare.Application.Services.Authentication;
public class TokenService(
    IAccessTokenGenerator accessTokenGenerator,
    IUnitOfWork unitOfWork)
    : ITokenService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<TokensDto> GenerateTokens(User user)
    {
        (string accessToken, Guid accessTokenIdentifier) = accessTokenGenerator.Generate(user: user);

        return new TokensDto
        {
            Access = accessToken
        };
    }
}
