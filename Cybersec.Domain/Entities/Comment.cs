using Cybersec.Domain.Commons;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cybersec.Domain.Entities;
public class Comment:Auditable
{
    [Required]
    public string Content { get; set; }

    // Foreign key to Article
    public long ArticleId { get; set; }

    [ForeignKey("Id")]
    public virtual Article Article { get; set; }

    // Foreign key to User
    public long UserId { get; set; }

    [ForeignKey("Id")]
    public virtual User User { get; set; }
}
