using Cybersec.Domain.Commons;
using System.Xml.Linq;

namespace Cybersec.Domain.Entities;
public class User:Auditable
{
    public string FirstName { get; set; }
    public string LastName { get; set; } 
    public string Email { get; set; }
    public string Password { get; set; }
    public string ImageUrl { get; set; }
    public bool isVerified { get; set; } = false;
    public string RefreshToken { get; set; } =string.Empty;
    public DateTime ExpireDate { get; set; }
    public ICollection<UserCode> USerCodes { get; set; }
    // Navigation property for Likes
    public virtual ICollection<Like> Likes { get; set; }

    // Navigation property for Comments
    public virtual ICollection<Comment> Comments { get; set; }
}
