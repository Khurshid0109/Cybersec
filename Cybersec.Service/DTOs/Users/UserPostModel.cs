using Cybersec.Service.DTOs.Auth;
using System.ComponentModel.DataAnnotations;

namespace Cybersec.Service.DTOs.Users;
public class UserPostModel:EmailModel
{
    [Required]
    [MinLength(2), MaxLength(20)]
    public string FirstName { get; set; }

    [Required]
    [MinLength(2), MaxLength(20)]   
    public string LastName { get; set; }

    [Required]
    public string Password { get; set; }
}
