using Cybersec.Domain.Enums;
using Cybersec.Service.ViewModels.Users;

namespace Cybersec.Service.Interfaces.Auth;
public interface IUserAuthentication
{
    Task<EmailExistance> CheckEmail(string email);
    Task<string> Login(LoginViewModel model);
    
}
