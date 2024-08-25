
using Cybersec.Domain.Commons;

namespace Cybersec.Domain.Entities;
public class UserCode:Auditable
{
    public long UserId { get; set; }
    public User User { get; set; }
    public int Code { get; set; }
    public bool IsUsed { get; set; } = false;
}
