using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cybersec.Data.IRepositories;
using Cybersec.Domain.Entities;
using Cybersec.Domain.Enums;
using Cybersec.Service.DTOs.Users;
using Cybersec.Service.Exceptions;
using Cybersec.Service.Extentions;
using Cybersec.Service.Helpers;
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

        if (user is not null && !user.isVerified)
            throw new CyberException(409, "Siz avval ro'yhatdan o'tgansiz, iltimos pochtangizni tasdiqlang va tizimga kiring!");

        if (user is not null)
            throw new CyberException(409, "Siz avval ro'yhatdan o'tgansiz, iltimos pochta va parol orqali tizimga kiring!");

        var mapped = mapper.Map<User>(model);
        mapped.CreatedAt = DateTime.UtcNow;
        mapped.ImageUrl = Path.Combine("StaticImages","StaticImages\admin.png");
        mapped.Password = HashPasswordHelper.PasswordHasher(model.Password);

        var result = await userRepository.InsertAsync(mapped);

        await existEmail.ResendCodeAsync(model.Email);

        return mapper.Map<UserViewModel>(result);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var user = await userRepository.SelectAll()
             .Where(u => u.Id == id)
             .FirstOrDefaultAsync();

        if (user is null)
            throw new CyberException(404, "User is not found.");

        await userRepository.DeleteAsync(id);

        return true;
    }

    public async Task<bool> RollbackAsync(long id)
    {
        var user = await userRepository.SelectAll()
             .IgnoreQueryFilters()
             .Where(u => u.Id == id)
             .FirstOrDefaultAsync();

        if (user is null || user.Status.Equals(Status.Active))
            throw new CyberException(404, "User is not found.");

        await userRepository.RollbackAsync(id);

        return true;
    } 

    public async Task<PaginationViewModel<UserViewModel>> GetAllAsync(PaginationParams @params, bool deleted = false)
    {
        var users = await userRepository.SelectAll()
               .IgnoreQueryFilters()
               .Where(u => deleted? u.Status == Status.Deleted : u.Status == Status.Active)
               .AsNoTracking()
               .ProjectTo<UserViewModel>(mapper.ConfigurationProvider)
               .ToPaginationAsync(@params);

        return users;
    }

    public async Task<UserViewModel> GetByEmailAsync(string email)
    {
        var user = await userRepository.SelectAll()
             .AsNoTracking()
             .AnyAsync(u => u.Email.ToLower() == email.ToLower());

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
            throw new CyberException(404,"User is not found.");

        return mapper.Map<UserViewModel>(user);
    }

    public async Task<UserViewModel> UpdateAsync(long id, UserPutModel model)
    {
        var user = await userRepository.SelectAll()
             .IgnoreQueryFilters()
             .FirstOrDefaultAsync(u => u.Id == id);

        if (user is null)
            throw new CyberException(404,"User is not found.");

        var mapped = mapper.Map(model, user);

        mapped =await userRepository.UpdateAsync(mapped);

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

        await userRepository.UpdateAsync(user);

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

            await userRepository.UpdateAsync(user);

            return true;
        }

        throw new CyberException(403, "Kod xato kiritildi!");
    }

    public async Task<UserViewModel> CreateByAdminAsync(UserPostModel model)
    {
        var user = await userRepository.SelectAll()
            .IgnoreQueryFilters()
            .Where(u => u.Email == model.Email)
            .FirstOrDefaultAsync();

        if (user is not null)
            throw new CyberException(409, "You have already registered,please Login!");

        var mapped = mapper.Map<User>(model);
        mapped.CreatedAt = DateTime.UtcNow;
        mapped.ImageUrl = Path.Combine("StaticImages","admin.png");
        mapped.isVerified = true;
        mapped.Password = HashPasswordHelper.PasswordHasher(model.Password);

        var result = await userRepository.InsertAsync(mapped);

        return mapper.Map<UserViewModel>(result);
    }
}
