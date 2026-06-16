using System.Net;

namespace PlanShare.Exceptions.ExceptionsBase;

public abstract class PlanShareException(string message) : SystemException(message: message)
{
    public abstract IList<string> GetErrorMessages();
    public abstract HttpStatusCode GetStatusCode();
}