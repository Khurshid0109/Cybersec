using Cybersec.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cybersec.Service.DTOs.Admins;
public class RegisterDto
{
    [Required]
    public string FullName { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    public Role Role { get; set; }
}
