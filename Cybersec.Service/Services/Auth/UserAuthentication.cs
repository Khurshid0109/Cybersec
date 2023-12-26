using Cybersec.Data.IRepositories;
using Cybersec.Service.Interfaces.Auth;
using Cybersec.Service.ViewModels.Users;

namespace Cybersec.Service.Services.Auth;
public class UserAuthentication : IUserAuthentication
{
    private readonly IUserRepository _userRepository;

    public UserAuthentication(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<string> Login(LoginViewModel model)
    {
        throw new NotImplementedException();
    }
}
