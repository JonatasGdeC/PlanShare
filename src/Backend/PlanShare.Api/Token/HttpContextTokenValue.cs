using PlanShare.Domain.Security.Tokens;

namespace PlanShare.Api.Token;

public class HttpContextTokenValue(IHttpContextAccessor httpContext) : ITokenProvider
{
    public string Value()
    {
        string authentication = httpContext.HttpContext!.Request.Headers.Authorization.ToString();

        return authentication["Bearer ".Length..].Trim();
    }
}