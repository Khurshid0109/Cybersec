using Cybersec.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Cybersec.Service.DTOs.Admins;
public class AdminPutModel
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public IFormFile? ImageUrl { get; set; }
    public Status Status { get; set; }
}
