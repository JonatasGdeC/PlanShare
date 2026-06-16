using System.Net;

namespace PlanShare.Exceptions.ExceptionsBase;

public class RefreshTokenExpiredException() : PlanShareException(message: ResourceMessagesException.EXPIRED_SESSION)
{
    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.Forbidden;
}
