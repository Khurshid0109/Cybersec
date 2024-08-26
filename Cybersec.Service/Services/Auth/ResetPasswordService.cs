using System.Web;
using Cybersec.Domain.Entities;
using Cybersec.Service.DTOs.Auth;
using Cybersec.Data.IRepositories;
using Cybersec.Service.Exceptions;
using Microsoft.EntityFrameworkCore;
using Cybersec.Service.Interfaces.Auth;
using Microsoft.Extensions.Configuration;
using Cybersec.Service.Helpers;

namespace Cybersec.Service.Services.Auth;
public class ResetPasswordService(
    IRepository<PasswordResetToken> repository,
    IUserRepository userRepository,
    IExistEmail existEmail,
    IConfiguration configuration)
    : IResetPasswordService
{

    public async Task<string> GeneratePasswordResetTokenAsync(string email)
    {
        var user = await userRepository.SelectAll()
                          .Where(x => x.Email.ToLower() == email.ToLower())
                          .FirstOrDefaultAsync();

        if (user is null)
            throw new CyberException(404, "Bunday foydalanuvchi mavjud emas.");
        // Generate a unique token
        var token = Guid.NewGuid().ToString();

        // Save the token in the database with the user ID and expiration time
        var passwordResetToken = new PasswordResetToken
        {
            UserId = user.Id,
            Token = token,
            ExpiryTime = DateTime.UtcNow.AddHours(0.5) // Token valid for 0.5 hour
        };

        // Construct the password reset link
        var baseUrl = configuration["BaseUrl"];
        var encodedToken = HttpUtility.UrlEncode(token);
        var resetLink = $"{baseUrl}/api/auth/reset-password?token={encodedToken}";

        // Construct the email message
        var message = new Message
        {
            Subject = "Iltimos pastdagi link orqali shaxsingizni tasdiqlang.",
            To = email,
            Body = $"Assalomu alaykum, Parolni qayta o'rnatish uchun quyidagi havolaga bosing: {resetLink} " +
            $"Havolaning yaroqlilik muddati 30 minut."
        };

        await existEmail.SendMessage(message);

        await repository.InsertAsync(passwordResetToken);

        return token;
    }

    public async Task<bool> VerifyPasswordResetTokenAsync(string token)
    {
        // Check if the token exists in the database and is valid
        var passwordResetToken = await repository.SelectAll()
            .AsNoTracking()
            .AnyAsync(t => t.Token == token && t.ExpiryTime > DateTime.UtcNow);

        return passwordResetToken;
    }

    public async Task<string> ChangePassword(string token, ResetPasswordModel model)
    {
        var passResetToken = await repository.SelectAll()
           .FirstOrDefaultAsync(t => t.Token == token && t.ExpiryTime > DateTime.UtcNow);

        if (passResetToken is null || passResetToken.IsUsed is true)
            throw new CyberException(404, "Bunday token mavjud emas.");

        var user = await userRepository.SelectAll()
           .Where(u => u.Id == passResetToken.UserId)
           .FirstOrDefaultAsync();

        if (user is null)
            throw new CyberException(404, "Bunday foydalanuvchi mavjud emas!");

        user.Password = HashPasswordHelper.PasswordHasher(model.Password1);
        await userRepository.UpdateAsync(user);

        passResetToken.IsUsed = true;
        await repository.UpdateAsync(passResetToken);

        return user.Email;
    }
}