using Cybersec.Domain.Enums;
using Cybersec.Data.IRepositories;
using Microsoft.EntityFrameworkCore;
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

    public async Task<EmailExistance> CheckEmail(string email)
    {
        var user = await _userRepository.SelectAll()
             .Where(u => u.Email.ToLower() == email.ToLower())
             .AsNoTracking()
             .FirstOrDefaultAsync();

        if (user is null)
            return EmailExistance.NotFound;

        if( user is not null && user.isVerified == false)
        {
            //Implement sent code here

            return EmailExistance.NotVerified;
        }

        return EmailExistance.Found;
    }

    public Task<string> Login(LoginViewModel model)
    {
        throw new NotImplementedException();
    }
}
