using Cybersec.Domain.Enums;

namespace Cybersec.Service.DTOs.Admins;
public class AdminViewModel
{
    public long Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string ImageUrl { get; set; }
    public Role Role { get; set; }
    public Status   Status { get; set; }
}
