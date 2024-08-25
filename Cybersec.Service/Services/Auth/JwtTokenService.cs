using Cybersec.Service.DTOs.Users;
using Cybersec.Service.Interfaces.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Cybersec.Service.Services.Auth;
public class JwtTokenService(
    IConfiguration configuration,
    ILogger<JwtTokenService> logger)
    : IJwtTokenService
{
    public Task<(string refreshToken, DateTime tokenValidityTime)> GenerateRefreshTokenAsync()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);

        if (!double.TryParse(configuration["JWT:RefreshTokenValidityHours"], out var refreshTokenValidityHours))
            refreshTokenValidityHours = 5;

        var tokenExpiryTime = DateTime.UtcNow.AddHours(refreshTokenValidityHours);
        return Task.FromResult((Convert.ToBase64String(randomNumber), tokenExpiryTime));
    }

    public Task<(string token, DateTime tokenExpiryTime)> GenerateTokenAsync(UserViewModel user)
    {

        ArgumentNullException.ThrowIfNull(user, nameof(user));

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(configuration["JWT:Key"]!);
        var expireDate = DateTime.UtcNow.AddMinutes(double.Parse(configuration["JWT:AccessTokenExpireMinutes"]!));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                 new("Id", user.Id.ToString()),
                 new("Email", user.Email),
                 new("FirstName", user.FirstName),
                 new("LastName", user.LastName),
                 new("Phone", user.Phone),
            ]),
            Audience = configuration["JWT:Audience"],
            Issuer = configuration["JWT:Issuer"],
            IssuedAt = DateTime.UtcNow,
            Expires = expireDate,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };


        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Task.FromResult((tokenHandler.WriteToken(token), expireDate));
    }

    public Task<UserViewModel?> GetUserByAccessTokenAsync(string accessToken)
    {
        if (string.IsNullOrEmpty(accessToken))
            return Task.FromResult<UserViewModel?>(null);

        var tokenHandler = new JwtSecurityTokenHandler();
        var secretKey = configuration["JWT:Key"] ?? throw new ArgumentNullException("Key");
        var key = Encoding.ASCII.GetBytes(secretKey);
        try
        {
            tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false, // true
                //ValidIssuer = _configuration["Jwt:ValidIssuer"],
                ValidateAudience = false, // true
                //ValidAudience = _configuration["Jwt:ValidAudience"],
                ClockSkew = TimeSpan.Zero
            }, out var validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            if (DateTime.UtcNow > jwtToken.ValidTo)
            {
                logger.LogInformation("Token has expired");
                throw new SecurityTokenExpiredException("Token has expired!");
            }
            //Enum.TryParse(jwtToken.Claims.First(x => x.Type == "Role").Value, true, out Role role);
            var user = new UserViewModel
            {
                Id = long.Parse(jwtToken.Claims.First(x => x.Type == "Id").Value),
                FirstName = jwtToken.Claims.First(x => x.Type == "FirstName").Value,
                LastName = jwtToken.Claims.First(x => x.Type == "LastName").Value,
                Phone = jwtToken.Claims.First(x => x.Type == "Phone").Value,
                Email = jwtToken.Claims.First(x => x.Type == "Email").Value,
                IsVerified = true
            };
            return Task.FromResult<UserViewModel?>(user);
        }
        catch (SecurityTokenExpiredException)
        {
            logger.LogInformation("Token has expired");
            throw;
            //return Task.FromResult<User?>(null);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error when validate token");
            return Task.FromResult<UserViewModel?>(null);
        }
    }
}
