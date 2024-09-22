using System.ComponentModel.DataAnnotations;

namespace Cybersec.Service.DTOs.Users;
public class UserPutModel
{
    [Required]
    [MinLength(2), MaxLength(20)]
    public string FirstName { get; set; }

    [Required]
    [MinLength(2), MaxLength(20)]
    public string LastName { get; set; }

    public string? ImageUrl { get; set; }
}
