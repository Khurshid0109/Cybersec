using Cybersec.Api.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Cybersec.Api.Attributes;
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class CustomAuthorizeAttribute() : Attribute, IAsyncAuthorizationFilter
{
    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {

        try
        {
            var user = context.HttpContext.GetUser();
            if (user is null)
            {
                context.Result = new RedirectToActionResult("ExistEmail", "Access", null);
                return Task.CompletedTask;
            }
           
            return Task.CompletedTask;
        }
        catch
        {
            context.Result = new RedirectToActionResult("ExistEmail", "Access", null);
            return Task.CompletedTask;
        }
    }
}
