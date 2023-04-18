using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

using WebApi.Services;
using DataLayer.Entities;

namespace WebApi.Controllers;

[Route("[controller]")]
[ApiController]
public class ApiController : Controller
{
    protected Account? Account { get; private set; }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var controller = (Controller)context.Controller;
        var authorizeAttr = controller.ControllerContext.ActionDescriptor.MethodInfo.GetCustomAttribute<AuthorizeAttribute>();
        
        // У action имеется атрибут Authorize
        if (authorizeAttr != null || controller.HttpContext.Request.Headers.ContainsKey("Authorization"))
        {
            if (!Authorize(controller.HttpContext))
                context.Result = StatusCode(401);

            if (authorizeAttr is { RequiredRoles.Length: > 0 } 
                && !authorizeAttr.RequiredRoles.Contains(Account!.Role.ToString()))
                context.Result = StatusCode(403);


        }

        base.OnActionExecuting(context);
    }

    protected bool Authorize(HttpContext httpContext)
    {
        if (httpContext.Request.Headers.TryGetValue("Authorization", out var value))
        {
            string token = value.ToString();
            if (!string.IsNullOrWhiteSpace(token)
                && token.StartsWith("Basic "))
            {
                var authorizationService = httpContext.RequestServices.GetRequiredService<IAuthorizationService>();
                return (Account = authorizationService.Authorize(token.Substring(6))) != null;
            }
        }
        return false;
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