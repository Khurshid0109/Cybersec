using Cybersec.Service.DTOs.Users;

namespace Cybersec.Api.Extentions;
public static class HttpContextExtention
{
    public static UserViewModel? GetUser(this HttpContext httpContext)
    {
        var user = httpContext.Items["User"] as UserViewModel;

        if(user is null)
            throw new UnauthorizedAccessException("User not found");

        return user;
    }
}
