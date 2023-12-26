using Cybersec.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Cybersec.Domain.Entities;
public class News
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Title { get; set; }

    [Required, MinLength(50)]
    public string Description { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime? UpdatedAt { get; set; }

    [Required]
    public Categories Category { get; set; }

    [Required]
    public string ImageUrl { get; set; }
}
