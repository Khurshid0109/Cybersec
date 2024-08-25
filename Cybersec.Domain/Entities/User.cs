using Cybersec.Domain.Commons;

namespace Cybersec.Domain.Entities;
public class User:Auditable
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string? ImageUrl { get; set; }
    public bool isVerified { get; set; } = false;
    public string RefreshToken { get; set; }
    public DateTime ExpireDate { get; set; }
    public ICollection<UserCode> USerCodes { get; set; }
}
