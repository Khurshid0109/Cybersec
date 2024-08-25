using Cybersec.Domain.Enums;

namespace Cybersec.Service.DTOs.Users;
public class UserViewModel
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    public bool IsVerified { get; set; }
    public Status Status { get; set; }
}
