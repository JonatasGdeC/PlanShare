using Microsoft.IdentityModel.Tokens;
using PlanShare.Domain.Security.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace PlanShare.Infrastructure.Security.Tokens.Access.Validator;

internal sealed class JwtTokenValidator : JwtTokenHandler, IAccessTokenValidator
{
    private readonly string _signingKey;

    public JwtTokenValidator(string signingKey) => _signingKey = signingKey;

    public Guid GetAccessTokenIdentifier(string token)
    {
        string identifier = GetClaimValue(token: token, claimType: JwtRegisteredClaimNames.Jti);

        return Guid.Parse(input: identifier);
    }

    public Guid GetUserIdentifier(string token)
    {
        string identifier = GetClaimValue(token: token, claimType: JwtRegisteredClaimNames.NameId);

        return Guid.Parse(input: identifier);
    }

    public void Validate(string token)
    {
        TokenValidationParameters validationParameters = new TokenValidationParameters()
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            IssuerSigningKey = SecurityKey(signingKey: _signingKey),
            ClockSkew = new TimeSpan(ticks: 0)
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        tokenHandler.ValidateToken(token: token, validationParameters: validationParameters, validatedToken: out _);
    }

    private static string GetClaimValue(string token, string claimType)
    {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        JwtSecurityToken? jwtToken = tokenHandler.ReadJwtToken(token: token);

        return jwtToken.Claims.First(predicate: claim => claim.Type == claimType).Value;
    }
}
