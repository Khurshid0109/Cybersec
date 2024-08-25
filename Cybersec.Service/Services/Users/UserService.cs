using AutoMapper;
using Cybersec.Data.IRepositories;
using Cybersec.Domain.Entities;
using Cybersec.Domain.Enums;
using Cybersec.Service.DTOs.Users;
using Cybersec.Service.Exceptions;
using Cybersec.Service.Interfaces.Auth;
using Cybersec.Service.Interfaces.Users;
using Microsoft.EntityFrameworkCore;

namespace Cybersec.Service.Services.Users;
public class UserService(
    IUserRepository userRepository,
    IMapper mapper,
    IExistEmail existEmail,
    IUserCodeRepository codeRepository)
    : IUserService
{
    public async Task<UserViewModel> CreateAsync(UserPostModel model)
    {
        var user = await userRepository.SelectAll()
            .IgnoreQueryFilters()
            .Where(u => u.Email == model.Email)
            .FirstOrDefaultAsync();

        if (user is not null && !user.IsVerified)
            throw new CyberException(409, "Siz avval ro'yhatdan o'tgansiz, iltimos pochtangizni tasdiqlang va tizimga kiring!");

        if (user is not null)
            throw new CyberException(409, "Siz avval ro'yhatdan o'tgansiz, iltimos pochta va parol orqali tizimga kiring!");

        var mapped = mapper.Map<User>(model);
        mapped.CreatedAt = DateTime.UtcNow;
        mapped.Password = HashPasswordHelper.PasswordHasher(model.Password);
        mapped.Role = Role.User;

        var result = await userRepository.InsertAsync(mapped);
        await userRepository.SaveAsync();

        await existEmail.ResendCodeAsync(model.Email);

        return mapper.Map<UserViewModel>(result);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var user = await userRepository.SelectAll()
             .IgnoreQueryFilters()
             .Where(u => u.Id == id)
             .FirstOrDefaultAsync();

        if (user is null)
            throw new CyberException(404, "User is not found.");

        await userRepository.DeleteAsync(u => u.Id == id);
        await userRepository.SaveAsync();

        return true;
    }

    public async Task<bool> RollbackAsync(long id)
    {
        var user = await userRepository.SelectAll()
             .IgnoreQueryFilters()
             .Where(u => u.Id == id)
             .FirstOrDefaultAsync();

        if (user is null)
            throw new CyberException(404, "User is not found.");

        await userRepository.RollbackAsync(u => u.Id == id);
        await userRepository.SaveAsync();

        return true;
    }

    public async Task<PaginationViewModel<UserViewModel>> GetAllAsync(PaginationParams @params, bool deleted = false)
    {
        var users = await userRepository.SelectAll()
               .IgnoreQueryFilters()
               .Where(u => u.IsDeleted == deleted)
               .AsNoTracking()
               .ProjectTo<UserViewModel>(mapper.ConfigurationProvider)
               .ToPaginationAsync(@params);

        return users;
    }

    public async Task<UserViewModel> GetByEmailAsync(string email)
    {
        var user = await userRepository.SelectAll()
             .AnyAsync(u => u.Email == email);

        if (!user)
            throw new CyberException(404, "Bunday foydalanuvchi topilmadi!");

        return mapper.Map<UserViewModel>(user);
    }

    public async Task<UserViewModel> GetByIdAsync(long id)
    {
        var user = await userRepository.SelectAll()
              .IgnoreQueryFilters()
              .Where(u => u.Id == id)
              .AsNoTracking()
              .FirstOrDefaultAsync();

        if (user is null)
            throw new CyberException(404);

        return mapper.Map<UserViewModel>(user);
    }

    public async Task<UserViewModel> UpdateAsync(long id, UserPutModel model)
    {
        var user = await userRepository.SelectAll()
             .IgnoreQueryFilters()
             .FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
            throw new CyberException(404);

        var mapped = mapper.Map(model, user);

        mapped = userRepository.Update(mapped);
        await userRepository.SaveAsync();

        return mapper.Map<UserViewModel>(mapped);
    }

    public async Task<bool> CheckPreviousPassword(long id, string password)
    {
        var user = await userRepository.SelectAll()
             .Where(u => u.Id == id)
             .AsNoTracking()
             .FirstOrDefaultAsync();

        if (user is null)
            throw new CyberException(404, "User is not found!");

        var result = HashPasswordHelper.IsEqual(password, user.Password);

        return result;
    }

    public async Task<bool> ChangePasswordAsync(long id, string oldPassword, string newPassword)
    {
        var user = await userRepository.SelectAll()
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync();

        if (user is null)
            throw new CyberException(404, "Foydalanuvchi topilmadi!");

        if (!HashPasswordHelper.IsEqual(oldPassword, user.Password))
            throw new CyberException(403, "Avvalgi parol xato kiritildi!");

        user.Password = HashPasswordHelper.PasswordHasher(newPassword);

        userRepository.Update(user);
        await userRepository.SaveAsync();

        return true;
    }

    public async Task<bool> ChangeEmailAsync(long userId, string email, long code)
    {
        var user = await userRepository.SelectAll()
            .Where(u => u.Id == userId)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        email = email.ToLower();

        if (user is null)
            throw new CyberException(404, "User is not found!");

        var userCodeAny = await codeRepository
           .SelectAll()
           .IgnoreQueryFilters()
           .Include(u => u.User)
           .AnyAsync(c => c.User.Email.ToLower() == email && c.ExpireDate > DateTime.UtcNow && c.Code == code);

        if (userCodeAny)
        {
            var newUser = await userRepository
                .SelectAll()
                .IgnoreQueryFilters()
                .AnyAsync(u => u.Email.ToLower() == email);

            if (newUser)
                throw new CyberException(403, "Bunday email allaqachon mavjud!");

            user.Email = email;

            userRepository.Update(user);
            await userRepository.SaveAsync();

            return true;
        }

        throw new CyberException(403, "Kod xato kiritildi!");
    }
}
