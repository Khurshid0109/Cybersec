using Cybersec.Service.DTOs.Admins;

namespace Cybersec.Service.Interfaces.Users;
public interface IAdminService
{
    Task<bool> LoginAsync(LoginDto loginDto);
    Task<bool> RegisterAsync(RegisterDto registerDto);
    Task<long> GetAdminIdFromClaimsAsync();
    Task<AdminViewModel> GetAdminByIdAsync(long id);
}
