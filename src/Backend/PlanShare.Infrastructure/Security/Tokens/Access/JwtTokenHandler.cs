using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PlanShare.Infrastructure.Security.Tokens.Access;
internal abstract class JwtTokenHandler
{
    protected static SymmetricSecurityKey SecurityKey(string signingKey)
    {
        byte[] symmetricKey = Encoding.UTF8.GetBytes(s: signingKey);
        return new SymmetricSecurityKey(key: symmetricKey);
    }
}
