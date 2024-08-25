using System.ComponentModel.DataAnnotations;

namespace Cybersec.Service.DTOs.Auth;
public class ResetPasswordModel
{
    [Required]
    [MinLength(6), MaxLength(10)]
    public string Password1 { get; set; }
    [Required]
    [MinLength(6), MaxLength(10)]
    public string Password2 { get; set; }
}
