using System.ComponentModel.DataAnnotations;

namespace Cybersec.Service.DTOs.Auth;
public class LoginPostModel:EmailModel
{
    [Required]
    public string Password { get; set; }
}
