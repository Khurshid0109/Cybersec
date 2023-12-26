using Cybersec.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cybersec.Service.ViewModels.News;
public class NewsModel
{
    [Key]
    public int Id { get; set; }

    public string Title { get; set; } 

    public string Description { get; set; } 

    public DateTime CreatedAt { get; set; } 

    public Categories Category { get; set; }

    public string ImageUrl { get; set; }
}
