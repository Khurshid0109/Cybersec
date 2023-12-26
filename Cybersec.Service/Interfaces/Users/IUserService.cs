using Cybersec.Service.ViewModels.Users;

namespace Cybersec.Service.Interfaces.Users;
public interface IUserService
{
    Task<bool> RemoveAsync(int id);
    Task<UserModel> RetrieveByIdAsync(int id);
    Task<UserModel> RetrieveByEmailAsync(string email);
    Task<IEnumerable<UserModel>> RetrieveAllAsync();
    Task<UserModel> CreateAsync(UserPostModel dto);
    Task<UserModel> ModifyAsync(int id, UserPutModel dto);
}
