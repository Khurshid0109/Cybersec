using Cybersec.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cybersec.Domain.Entities;
public class News
{
    [Key]
    public int Id { get; set; }

    [Required, MinLength(2),MaxLength(100)]
    public string Title { get; set; }

    [Required, MinLength(50)]
    public string Description { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? UpdatedAt { get; set; }

    [Required]
    public Categories Category { get; set; }

    [Required,MinLength(6),MaxLength(50)]
    public string SourceLink { get; set; }
    
    public List<string> ImageUrls { get; set; }
    public List<string> VideoUrls { get; set; }
}
