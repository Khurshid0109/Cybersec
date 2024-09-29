using Cybersec.Service.DTOs.Users;
using Cybersec.Service.Extentions;

namespace Cybersec.Service.Interfaces.Users;
public interface IUserService
{
    Task<bool> DeleteAsync(long id);
    Task<bool> RollbackAsync(long id);
    Task<UserViewModel> GetByIdAsync(long id);
    Task<PaginationViewModel<UserViewModel>> GetAllAsync(PaginationParams @params, bool deleted);
    Task<UserViewModel> GetByEmailAsync(string email);
    Task<UserViewModel> CreateAsync(UserPostModel model);
    Task<UserViewModel> CreateByAdminAsync(UserPostModel model);
    Task<UserViewModel> UpdateAsync(long id, UserPutModel model);
    Task<UserViewModel> UpdateByAdminAsync(long id, UserByAdminPutModel model);
    Task<bool> CheckPreviousPassword(long id, string password);
    Task<bool> ChangePasswordAsync(long id, string oldPassword, string newPassword);
    Task<bool> ChangeEmailAsync(long userID, string email, long code);
}
