using System.ComponentModel.DataAnnotations;

namespace Cybersec.Service.DTOs.Admins;
public class LoginDto
{
    [Required]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}
