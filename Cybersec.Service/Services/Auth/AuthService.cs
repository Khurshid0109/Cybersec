using AutoMapper;
using Cybersec.Domain.Enums;
using Cybersec.Service.Helpers;
using Cybersec.Domain.Entities;
using Cybersec.Service.DTOs.Auth;
using Cybersec.Service.DTOs.Users;
using Cybersec.Data.IRepositories;
using Cybersec.Service.Exceptions;
using Microsoft.EntityFrameworkCore;
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
        var user = await userRepository.SelectAll()
            .Where(u => u.Email.ToLower().Equals(login.Email.ToLower()))
            .FirstOrDefaultAsync();

        if (user is null || !HashPasswordHelper.IsEqual(login.Password, user.Password))
            throw new CyberException(404, "Email yoki parol xato!");

        if (user.Status == Status.Deleted)
            throw new CyberException(403, "Sizning hisobingiz bloklangan!");

        if (!user.isVerified)
            throw new CyberException(403, "Iltimos avval pochtangizni tasdiqlang!");

        (user.RefreshToken, user.ExpireDate) = await jwtTokenService.GenerateRefreshTokenAsync();

        await userRepository.UpdateAsync(user);

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
        var user = await userRepository.SelectAll()
            .Where(u => u.Email.ToLower().Equals(model.Email.ToLower()))
            .FirstOrDefaultAsync();

        if (user is not null && !user.isVerified)
            throw new CyberException(409, "Siz avval ro'yhatdan o'tgansiz, iltimos pochtangizni tasdiqlang va tizimga kiring!");

        if (user is not null)
            throw new CyberException(409, "Siz avval ro'yhatdan o'tgansiz, iltimos pochta va parol orqali tizimga kiring!");

        var mapped = mapper.Map<User>(model);
        mapped.CreatedAt = DateTime.UtcNow;
        mapped.Password = HashPasswordHelper.PasswordHasher(model.Password);

        var result = await userRepository.InsertAsync(mapped);

        await existEmail.ResendCodeAsync(model.Email);

        var userView = mapper.Map<UserViewModel>(result);
        return userView;
    }

    public async Task<bool> ResetPassword(string email)
    {
        var user = await userRepository.SelectAll()
            .Where(u => u.Email.ToLower().Equals(email.ToLower()))
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (user is null)
            return false;

        return await existEmail.ResendCodeAsync(email);
    }

    public async Task<bool> CheckResetPasswordCode(string email, long code)
    {
        var userCodeAny = await userCodeRepository.SelectAll()
           .IgnoreQueryFilters()
           .Where(c => c.ExpireDate > DateTime.UtcNow && c.Code == code)
           .AnyAsync(c => c.User.Email == email && c.User.isVerified == true);

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
        var user = await userRepository.SelectAll()
            .Where(u => u.Id == model.UserId && u.RefreshToken == model.RefreshToken
                && u.ExpireDate > DateTime.UtcNow)
            .FirstOrDefaultAsync();

        if (user is null)
            throw new CyberException(401, "Xavfsizlik sabab tizimga qayta kiring!");

        var userView = mapper.Map<UserViewModel>(user);
        var tokenTask = jwtTokenService.GenerateTokenAsync(userView);
        (user.RefreshToken, user.ExpireDate) = await jwtTokenService.GenerateRefreshTokenAsync();

       await userRepository.UpdateAsync(user);

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
