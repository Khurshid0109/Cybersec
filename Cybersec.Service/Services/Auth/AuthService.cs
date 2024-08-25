using AutoMapper;
using Cybersec.Data.IRepositories;
using Cybersec.Domain.Entities;
using Cybersec.Service.DTOs.Auth;
using Cybersec.Service.DTOs.Users;
using Cybersec.Service.Exceptions;
using Cybersec.Service.Interfaces.Auth;

namespace Cybersec.Service.Services.Auth;
public class AuthService(
    IUserRepository userRepository,
    IJwtTokenService jwtTokenService,
    IMapper mapper,
    IExistEmail existEmail,
    IUserCodeRepository userCodeRepository)
    : IAuthService
{
    public async Task<LoginViewModel> AuthenticateAsync(LoginPostModel login)
    {
        var user = await userRepository.SelectAsync(u => u.Email == login.Email, deleted: true, asNoTracking: true);

        if (user is null || !HashPasswordHelper.IsEqual(login.Password, user.Password))
            throw new CyberException(404, "Email yoki parol xato!");

        if (user.IsDeleted)
            throw new CyberException(403, "Sizning hisobingiz bloklangan!");

        if (!user.IsVerified)
            throw new CyberException(403, "Iltimos avval pochtangizni tasdiqlang!");

        (user.RefreshToken, user.ExpireDate) = await jwtTokenService.GenerateRefreshTokenAsync();

        await userRepository.BulkUpdateAsync(u => u.Email == login.Email,
            setters => setters
                .SetProperty(e => e.RefreshToken, user.RefreshToken)
                .SetProperty(e => e.ExpireDate, user.ExpireDate));

        //await userService.UpdateAsync(user.Id, mapper.Map<UserPutModel>(user));

        var userView = mapper.Map<UserViewModel>(user);
        (string token, DateTime expireDate) = await jwtTokenService.GenerateTokenAsync(userView);
        return new LoginViewModel
        {
            Token = token,
            AccessTokenExpireDate = expireDate,
            RefreshToken = user.RefreshToken,
            User = userView
        };
    }

    public async Task<UserViewModel> CreateAsync(UserPostModel model)
    {
        var user = await userRepository.SelectAsync(
            u => u.Email == model.Email,
            deleted: true,
            asNoTracking: true);

        if (user is not null && !user.IsVerified)
            throw new CyberException(409, "Siz avval ro'yhatdan o'tgansiz, iltimos pochtangizni tasdiqlang va tizimga kiring!");

        if (user is not null)
            throw new CyberException(409, "Siz avval ro'yhatdan o'tgansiz, iltimos pochta va parol orqali tizimga kiring!");

        var mapped = mapper.Map<User>(model);
        mapped.CreatedAt = DateTime.UtcNow;
        mapped.Password = HashPasswordHelper.PasswordHasher(model.Password);

        var result = await userRepository.InsertAsync(mapped);
        await userRepository.SaveAsync();

        await existEmail.ResendCodeAsync(model.Email);

        var userView = mapper.Map<UserViewModel>(result);
        return userView;
    }

    public async Task<bool> ResetPassword(string email)
    {
        var user = await userRepository.SelectAsync(u => u.Email == email, asNoTracking: true);
        if (user is null)
            return false;
        return await existEmail.ResendCodeAsync(email);
    }

    public async Task<bool> CheckResetPasswordCode(string email, long code)
    {
        var userCodeAny = await userCodeRepository
           .SelectAll(c => c.ExpireDate > DateTime.UtcNow && c.Code == code, deleted: true, includes: ["Users"])
           .AnyAsync(c => c.User.Email == email && c.User.IsVerified == true);

        if (userCodeAny)
        {
            // logic
            // emailga parolni yangilash linkini yuborish
            return true;
        }

        return false;
    }

    public async Task<LoginViewModel> RefreshTokenAsync(RefreshTokenPostModel model)
    {
        var user = await userRepository.SelectAsync(
            u => u.Id == model.UserId
                && u.RefreshToken == model.RefreshToken
                && u.ExpireDate > DateTime.UtcNow,
            asNoTracking: true);

        if (user is null)
            throw new CyberException(401, "Xavfsizlik sabab tizimga qayta kiring!");

        var userView = mapper.Map<UserViewModel>(user);
        var tokenTask = jwtTokenService.GenerateTokenAsync(userView);
        (user.RefreshToken, user.ExpireDate) = await jwtTokenService.GenerateRefreshTokenAsync();

        await userRepository.BulkUpdateAsync(u => u.Email == user.Email,
            setters => setters
                .SetProperty(e => e.RefreshToken, user.RefreshToken)
                .SetProperty(e => e.ExpireDate, user.ExpireDate));

        (string token, DateTime expireDate) = await tokenTask;

        return new LoginViewModel
        {
            Token = token,
            AccessTokenExpireDate = expireDate,
            RefreshToken = user.RefreshToken,
            User = userView
        };
    }
}
