using Cybersec.Service.ViewModels.Users;

namespace Cybersec.Service.ViewModels.Code;
public class UserCodeViewModel
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public UserModel User { get; set; }
    public int Code { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsUsed { get; set; } 
}
