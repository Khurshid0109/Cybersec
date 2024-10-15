using Cybersec.Service.Interfaces.Auth;

namespace Cybersec.Api.Middlewares;
public class JwtMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IJwtTokenService jwtTokenService, ILogger<JwtMiddleware> logger)
    {
        var accessToken = context.Request.Cookies["accessToken"];
        try
        {
            if (!string.IsNullOrEmpty(accessToken))
            {
                var user = await jwtTokenService.GetUserByAccessTokenAsync(accessToken ?? string.Empty);
                context.Items["User"] = user;
            }
            else
                context.Items["User"] = null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "JwtMiddleware error! Message: {Message}", ex.Message);
        }
        finally
        {
            await next(context);
        }
    }
}
