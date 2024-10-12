using AutoMapper;
using Cybersec.Domain.Enums;
using System.Security.Claims;
using Cybersec.Service.Helpers;
using Cybersec.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Cybersec.Service.Extentions;
using Cybersec.Data.IRepositories;
using Cybersec.Service.Exceptions;
using Cybersec.Service.DTOs.Admins;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using Cybersec.Service.Interfaces.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Cybersec.Service.Services.Users;
public class AdminService : IAdminService
{
    private readonly IAdminRepository _adminRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly string _sharedMediaPath;

    public AdminService(IAdminRepository adminRepository,
                        IHttpContextAccessor httpContextAccessor,
                        IConfiguration configuration,
                        IMapper mapper)
    {
        _adminRepository = adminRepository;
        _httpContextAccessor = httpContextAccessor;
        _sharedMediaPath = configuration["SharedMedia:MediaPath"]?? "D:\\Projects\\Cybersec\\Cybersec.SharedResources\\Shared";
        _mapper = mapper;
    }

    public async Task<bool> LoginAsync(LoginDto loginDto)
    {
        var admin = await _adminRepository.SelectAll()
            .Where(a => a.Email.ToLower().Equals(loginDto.Email.ToLower()))
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (admin is null)
            throw new CyberException(404, "Invalid email or password");

        if (HashPasswordHelper.IsEqual(loginDto.Password, admin.Password))
        {
            var claims = new List<Claim>
            {
                new Claim("Id",admin.Id.ToString()),
                new Claim(ClaimTypes.Name, admin.FullName),
                new Claim(ClaimTypes.Role, admin.Role.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true, // Keeps the user logged in across sessions
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(0.5) // Set cookie expiration
            };

            await _httpContextAccessor.HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties
                        );

            return true;
        }

        return false;
    }

    public async Task<bool> RegisterAsync(RegisterDto registerDto)
    {
        var admin = await _adminRepository.SelectAll()
            .Where(a => a.Email.ToLower().Equals(registerDto.Email.ToLower()))
            .FirstOrDefaultAsync();

        if (admin is not null)
            return false;

        var newAdmin = new Admin
        {
            Email = registerDto.Email,
            Password = HashPasswordHelper.PasswordHasher(registerDto.Password),
            Role = registerDto.Role
        };

        await _adminRepository.InsertAsync(newAdmin);

        return true;
    }

    public async Task<long> GetAdminIdFromClaimsAsync()
    {
        var context = _httpContextAccessor.HttpContext;

        if (context is null || !context.User.Identity.IsAuthenticated)
        {
            throw new InvalidOperationException("User is not authenticated.");
        }

        var adminIdClaim = context.User.FindFirst("Id")?.Value;

        if (string.IsNullOrEmpty(adminIdClaim))
        {
            throw new InvalidOperationException("Admin ID is not found in claims.");
        }

        return await Task.FromResult(long.Parse(adminIdClaim));
    }

    public async Task<AdminViewModel> GetAdminByIdAsync(long id)
    {
        var admin = await _adminRepository.SelectAll()
            .Where(a => a.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (admin is null)
            throw new CyberException(404, "Admin is not found.");

        return _mapper.Map<AdminViewModel>(admin);
    }

    public async Task<PaginationViewModel<AdminViewModel>> GetAllAsync(PaginationParams @params)
    {
        var admins = await _adminRepository.SelectAll()
               .IgnoreQueryFilters()
               .AsNoTracking()
               .ProjectTo<AdminViewModel>(_mapper.ConfigurationProvider)
               .ToPaginationAsync(@params);

        return _mapper.Map<PaginationViewModel<AdminViewModel>>(admins);
    }

    public async Task<bool> DeleteAdminAsync(long id)
    {
        var admin = await _adminRepository.SelectAll()
            .Where(a => a.Id == id)
            .FirstOrDefaultAsync();

        if(admin is null)
            throw new CyberException(404,"Admin is not found.");

        var result =await _adminRepository.DeleteAsync(id);

        return result;
    }

    public async Task<AdminViewModel> AddAdminAsync(AdminPostModel model)
    {
        var admin = await _adminRepository.SelectAll()
             .Where(a => a.Email.ToLower().Equals(model.Email.ToLower()))
             .FirstOrDefaultAsync();

        if (admin is not null)
            throw new CyberException(409, "User is already exists.");

        var mapped = _mapper.Map<Admin>(model);
        mapped.CreatedAt = DateTime.UtcNow;
        mapped.Role = Role.Admin;
        mapped.Password = HashPasswordHelper.PasswordHasher(model.Password);
        mapped.ImageUrl = Path.Combine("Images",await MediaHelper.UploadFile(model.ImageUrl, "image"));

        var result = await _adminRepository.InsertAsync(mapped);
        return _mapper.Map<AdminViewModel>(result);
    }

    public async Task<AdminViewModel> UpdateAdminAsync(long id, AdminPutModel model)
    {
        var admin = await _adminRepository.SelectByIdAsync(id);

        if (admin is null)
            throw new CyberException(404, "Admin is not found.");

        var mapped = _mapper.Map(model, admin);
        mapped.UpdatedAt = DateTime.UtcNow;
        mapped.Password = HashPasswordHelper.PasswordHasher(model.Password);
        mapped.ImageUrl = Path.Combine("Images", await MediaHelper.UploadFile(model.ImageUrl, "image"));

        var result = await _adminRepository.UpdateAsync(mapped);
        return _mapper.Map<AdminViewModel>(mapped);
    }

    public async Task<AdminViewModel> UpdateAdminSettingsAsync(long id,AdminSettingsModel model)
    {
        var admin = await _adminRepository.SelectAll()
            .Where(a => a.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (admin is null)
            throw new CyberException(404, "Admin is not found!");

        var mapped = _mapper.Map(model, admin);
        mapped.UpdatedAt = DateTime.UtcNow;

        if (model.ImageUrl is not null)
            mapped.ImageUrl = Path.Combine("Images",await MediaHelper.UploadFile(model.ImageUrl, "image"));

        var result = await _adminRepository.UpdateAsync(mapped);

        return _mapper.Map<AdminViewModel>(result);
    }

    public async Task LogoutAsync()
    {
        await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

}
