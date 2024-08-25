using Cybersec.Service.DTOs.Users;

namespace Cybersec.Service.DTOs.Auth;
public class LoginViewModel:EmailModel
{
    public string Token { get; set; }
    public DateTime AccessTokenExpireDate { get; set; }
    public string RefreshToken { get; set; }
    public UserViewModel User { get; set; }
}
