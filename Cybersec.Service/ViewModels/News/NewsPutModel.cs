using Cybersec.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Cybersec.Service.ViewModels.News;
public class NewsPutModel
{
    [Required, MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required, MinLength(50)]
    public string Description { get; set; } = string.Empty;

    [DataType(DataType.DateTime)]
    public DateTime CreatedTime { get; set; } = DateTime.UtcNow;

    [Required]
    public Categories Category { get; set; }

    [Required]
    public string SourceLink { get; set; }

    [Required]
    public IFormFile ImageUrl { get; set; }
}
