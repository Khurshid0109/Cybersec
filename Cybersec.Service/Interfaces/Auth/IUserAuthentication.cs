using Cybersec.Service.ViewModels.Users;

namespace Cybersec.Service.Interfaces.Auth;
public interface IUserAuthentication
{
    Task<string> Login(LoginViewModel model);
}
