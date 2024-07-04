using Cybersec.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Cybersec.Service.ViewModels.News;
public class NewsPostModel
{
    [Required, MaxLength(100)]
    public string Title { get; set; }

    [Required, MinLength(50)]
    public string Description { get; set; }

    [Required]
    public Categories Category { get; set; }

    public string SourceLink { get; set; }

    public List<IFormFile> Images { get; set; } 
    public List<IFormFile> Videos { get; set; } 
}
