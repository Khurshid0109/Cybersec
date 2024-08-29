using AutoMapper;
using Cybersec.Data.IRepositories;
using Cybersec.Domain.Entities;
using Cybersec.Service.DTOs.Admins;
using Cybersec.Service.Exceptions;
using Cybersec.Service.Helpers;
using Cybersec.Service.Interfaces.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Cybersec.Service.Services.Users;
public class AdminService : IAdminService
{
    private readonly IAdminRepository _adminRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public AdminService(IAdminRepository adminRepository, 
                        IHttpContextAccessor httpContextAccessor, 
                        IMapper mapper)
    {
        _adminRepository = adminRepository;
        _httpContextAccessor = httpContextAccessor;
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

        // Simulate an async operation, even though this is not necessary for claims retrieval.
        return await Task.FromResult(long.Parse(adminIdClaim));
    }

    public async Task<AdminViewModel> GetAdminByIdAsync(long id)
    {
        var admin =await _adminRepository.SelectAll()
            .Where(a => a.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();

        if (admin is null)
            throw new CyberException(404, "Admin is not found.");

        return _mapper.Map<AdminViewModel>(admin);
    }
}
