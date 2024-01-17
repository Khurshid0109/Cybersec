using Cybersec.Domain.Enums;
using Cybersec.Domain.Entities;
using Cybersec.Service.ViewModels.Users;

namespace Cybersec.Service.Interfaces.Auth;
public interface IUserAuthentication
{
    Task<EmailExistance> CheckEmail(string email);
    Task<string> Login(LoginViewModel model);
    Task<string> Register(UserPostModel model);
    public Task<bool> GenerateCode(User user);
    public Task SendMessage(Message message);

}
