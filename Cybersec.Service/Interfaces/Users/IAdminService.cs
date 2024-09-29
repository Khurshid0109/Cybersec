using Cybersec.Service.Extentions;
using Cybersec.Service.DTOs.Admins;

namespace Cybersec.Service.Interfaces.Users;
public interface IAdminService
{
    Task<bool> LoginAsync(LoginDto loginDto);
    Task<bool> RegisterAsync(RegisterDto registerDto);
    Task<long> GetAdminIdFromClaimsAsync();
    Task<AdminViewModel> GetAdminByIdAsync(long id);
    Task<PaginationViewModel<AdminViewModel>> GetAllAsync(PaginationParams @params);
    Task<bool> DeleteAdminAsync(long id);
    Task<AdminViewModel> AddAdminAsync(AdminPostModel model);
    Task<AdminViewModel> UpdateAdminAsync(long id, AdminPutModel model);
    Task LogoutAsync();
}
