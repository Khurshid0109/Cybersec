using Cybersec.Domain.Enums;

namespace Cybersec.Service.DTOs.Users;
public class UserByAdminPutModel
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? ImageUrl { get; set; }
    public Status Status { get; set; }
}
