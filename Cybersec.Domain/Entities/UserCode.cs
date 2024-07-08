
namespace Cybersec.Domain.Entities;
public class UserCode
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public User User { get; set; }
    public int Code { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsUsed { get; set; } = false;
}
