using Cybersec.Api.Helpers;
using Cybersec.Service.Exceptions;

namespace Cybersec.Api.Middlewares;
public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
            if (context.Response.StatusCode == StatusCodes.Status404NotFound)
                context.Response.Redirect("/ErrorHandler/GlobalError?statusCode=404");
        }
        catch(CyberException ex)
        {
             context.Response.Redirect($"/ErrorHandler/GlobalError?statusCode={ex.StatusCode}");

        }
        catch (Exception e)
        {
            context.Response.Redirect($"/ErrorHandler/GlobalError?statusCode=500");
        }
    }
}
