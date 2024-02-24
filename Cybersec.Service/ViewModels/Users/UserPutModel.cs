using Microsoft.AspNetCore.Http;

namespace Cybersec.Service.ViewModels.Users;
public class UserPutModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public IFormFile ImageUrl { get; set; }
}
