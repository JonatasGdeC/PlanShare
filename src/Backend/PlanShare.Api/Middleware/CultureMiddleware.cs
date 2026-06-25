using PlanShare.Domain.Extensions;
using System.Globalization;

namespace PlanShare.Api.Middleware;

public class CultureMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        CultureInfo[] supportedLanguages = CultureInfo.GetCultures(types: CultureTypes.AllCultures);

        string? culture = context.Request.Headers.AcceptLanguage.FirstOrDefault();

        CultureInfo cultureInfo = new CultureInfo(name: "en");

        if (culture.NotEmpty() && supportedLanguages.Any(predicate: s => s.Name.Equals(value: culture)))
        {
            cultureInfo = new CultureInfo(name: culture);
        }

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await next(context: context);
    }
}