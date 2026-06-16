using Microsoft.EntityFrameworkCore;
using PlanShare.Domain.Entities;
using PlanShare.Domain.Security.Tokens;
using PlanShare.Domain.Services.LoggedUser;
using PlanShare.Infrastructure.DataAccess;
using System.IdentityModel.Tokens.Jwt;

namespace PlanShare.Infrastructure.Services.LoggedUser;
internal sealed class LoggedUser(PlanShareDbContext dbContext, ITokenProvider tokenValue) : ILoggedUser
{
    public async Task<User> Get()
    {
        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

        JwtSecurityToken? jwtSecurityToken = tokenHandler.ReadJwtToken(token: tokenValue.Value());

        string identifier = jwtSecurityToken.Claims.First(predicate: claim => claim.Type == JwtRegisteredClaimNames.NameId).Value;

        return await dbContext
            .Users
            .AsNoTracking()
            .FirstAsync(predicate: user => user.Active && user.Id == Guid.Parse(identifier));
    }
}