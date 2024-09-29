using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Cybersec.Admin.Middlewares
{
    public class AuthRedirectMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthRedirectMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value.ToLower();

            // Allow unauthenticated access to root and login page
            if (path == "/" || path.Contains("/access/login"))
            {
                await _next(context);
                return;
            }

            // Check for the authentication cookie
            var cookie = context.Request.Cookies["CybersecAuth"];

            // If the cookie does not exist, redirect to login
            if (string.IsNullOrEmpty(cookie))
            {
                context.Response.Redirect("/Access/Login");
                return;
            }

            // Decode the cookie (assuming it's base64 encoded)
            var decodedBytes = Convert.FromBase64String(cookie);
            var decodedString = Encoding.UTF8.GetString(decodedBytes);

            // Here you can deserialize the claims (assuming JSON format)
            var claims = JsonSerializer.Deserialize<Dictionary<string, string>>(decodedString);

            if (claims == null || !claims.ContainsKey("Id") || !claims.ContainsKey(ClaimTypes.Expiration))
            {
                context.Response.Redirect("/Access/Login");
                return;
            }

            var userId = claims["Id"];
            var expirationClaim = claims[ClaimTypes.Expiration];

            // Check expiration time
            if (DateTime.TryParse(expirationClaim, out var expirationTime))
            {
                if (expirationTime < DateTime.UtcNow)
                {
                    // Cookie has expired
                    context.Response.Redirect("/Access/Login");
                    return;
                }
            }
            else
            {
                // Invalid expiration format, redirect
                context.Response.Redirect("/Access/Login");
                return;
            }

            // Optionally, you can re-add the claims to the context.User for use in the application
            var identity = new ClaimsIdentity(claims.Select(c => new Claim(c.Key, c.Value)), CookieAuthenticationDefaults.AuthenticationScheme);
            context.User = new ClaimsPrincipal(identity);

            // Proceed to the next middleware if authenticated and cookie is valid
            await _next(context);
        }
    }
}
