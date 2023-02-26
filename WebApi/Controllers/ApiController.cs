using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class ApiController : Controller
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // TODO
        base.OnActionExecuting(context);
    }

    //
    // Методы для получения параметра GET запроса
    //
    protected string? GetQueryParameterString(string key)
    {
        if (Request.Query.TryGetValue(key, out var value) && value.Count == 1)
            return value[0];
        return null;
    }

    protected int? GetQueryParameterInt32(string key)
    {
        string? parameterString = GetQueryParameterString(key);
        if (parameterString is not null
            && int.TryParse(parameterString, out int value))
            return value;
        return null;
    }

    protected long? GetQueryParameterInt64(string key)
    {
        string? parameterString = GetQueryParameterString(key);
        if (parameterString is not null
            && long.TryParse(parameterString, out long value))
            return value;
        return null;
    }

    protected DateTime? GetQueryParameterDateTime(string key)
    {
        string? parameterString = GetQueryParameterString(key);
        if (parameterString is not null
            && DateTime.TryParse(parameterString, out DateTime value))
            return value;
        return null;
    }
}