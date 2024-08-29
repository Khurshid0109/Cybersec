using Cybersec.Domain.Enums;
using Cybersec.Domain.Commons;

namespace Cybersec.Domain.Entities;
public class Admin:Auditable
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string ImageUrl { get; set; }
    public Role Role { get; set; }
    public Status Status { get; set; }
}
