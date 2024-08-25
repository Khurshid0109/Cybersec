using Cybersec.Domain.Commons;

namespace Cybersec.Domain.Entities;
public class PasswordResetToken:Auditable
{
    public long UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiryTime { get; set; }
    public bool IsUsed { get; set; } = false;

    public virtual User User { get; set; }
}
