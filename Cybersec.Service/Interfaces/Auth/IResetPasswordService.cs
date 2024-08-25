using Cybersec.Service.DTOs.Auth;

namespace Cybersec.Service.Interfaces.Auth;
public interface IResetPasswordService
{
    Task<string> GeneratePasswordResetTokenAsync(string email);
    Task<bool> VerifyPasswordResetTokenAsync(string token);
    Task<string> ChangePassword(string token, ResetPasswordModel model);
}
