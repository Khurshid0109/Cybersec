using Cybersec.Domain.Enums;

namespace Cybersec.Domain.Entities;
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string ImageUrl { get; set; }
    public bool isVerified { get; set; } = false;
    public Role Role { get; set; }
    public Status Status { get; set; }
    public ICollection<UserCode> USerCodes { get; set; }
}
