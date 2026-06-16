using System.Net;

namespace PlanShare.Exceptions.ExceptionsBase;
public class NotFoundException(string mensagem) : PlanShareException(message: mensagem)
{
    public override IList<string> GetErrorMessages() => [Message];

    public override HttpStatusCode GetStatusCode() => HttpStatusCode.NotFound;
}
