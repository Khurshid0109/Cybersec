using Cybersec.Domain.Entities;
using Cybersec.Service.DTOs.Code;

namespace Cybersec.Service.Interfaces.Users;
public interface IUserCodeService
{
    Task<bool> DeleteAsync(long id);
    Task<UserCodeViewModel> GetByIdAsync(long id);
    Task<UserCodeViewModel> CreateAsync(UserCode model);
    Task<PaginationViewModel<UserCodeViewModel>> GetAllAsync(PaginationParams @params, bool deleted);
}
