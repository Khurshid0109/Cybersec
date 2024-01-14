
namespace Cybersec.Domain.Entities;
public class UserCode
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int Code { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsUsed { get; set; } = false;
}
