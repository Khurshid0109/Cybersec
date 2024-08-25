using System.ComponentModel.DataAnnotations;

namespace Cybersec.Service.DTOs.Auth;
public class RefreshTokenPostModel
{
    public long UserId { get; set; }
    [Required]
    public string RefreshToken { get; set; }
}
