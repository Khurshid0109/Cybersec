using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Cybersec.Service.DTOs.Admins;
public class AdminSettingsModel
{
    [Required]
    [MaxLength(30)]
    [MinLength(3)]
    public string FullName { get; set; }

    public IFormFile? ImageUrl { get; set; }
}
