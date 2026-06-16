using Microsoft.IdentityModel.Tokens;
using PlanShare.Domain.Entities;
using PlanShare.Domain.Security.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PlanShare.Infrastructure.Security.Tokens.Access.Generator;

internal sealed class JwtTokenGenerator(uint expirationTimeMinutes, string signingKey)
    : JwtTokenHandler, IAccessTokenGenerator
{
    public (string token, Guid accessTokenIdentifier) Generate(User user)
    {
        Guid accessTokenIdentifier = Guid.NewGuid();

        List<Claim> claims = new List<Claim>
        {
            new (type: JwtRegisteredClaimNames.Jti, value: accessTokenIdentifier.ToString()),
            new (type: JwtRegisteredClaimNames.NameId, value: user.Id.ToString())
        };

        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(value: expirationTimeMinutes),
            Subject = new ClaimsIdentity(claims: claims),
            SigningCredentials = new SigningCredentials(key: SecurityKey(signingKey: signingKey), algorithm: SecurityAlgorithms.HmacSha256Signature)
        };

        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        SecurityToken? securityToken = tokenHandler.CreateToken(tokenDescriptor: tokenDescriptor);

        return (tokenHandler.WriteToken(token: securityToken), accessTokenIdentifier);
    }
}
