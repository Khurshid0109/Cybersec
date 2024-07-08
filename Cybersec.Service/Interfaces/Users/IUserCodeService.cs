using Cybersec.Service.ViewModels.Code;

namespace Cybersec.Service.Interfaces.Users;
public interface IUserCodeService
{
    Task<bool> RemoveAsync(int id);
    Task<UserCodeViewModel> RetrieveByIdAsync(int id);
    Task<IEnumerable<UserCodeViewModel>> RetrieveAllAsync();
    Task<UserCodeViewModel> CreateAsync(UserCodePostModel dto);
}
