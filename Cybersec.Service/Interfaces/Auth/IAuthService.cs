using Cybersec.Service.DTOs.Auth;
using Cybersec.Service.DTOs.Users;

namespace Cybersec.Service.Interfaces.Auth;
public interface IAuthService
{
    public Task<LoginViewModel> AuthenticateAsync(LoginPostModel login);
    public Task<bool> SaveAccessTokenAsync(LoginViewModel model);
    public Task<UserViewModel> CreateAsync(UserPostModel user);
    public Task<bool> ResetPassword(string email);
    public Task<bool> CheckResetPasswordCode(string email, long code);
    public Task<LoginViewModel> RefreshTokenAsync(RefreshTokenPostModel model);
}
