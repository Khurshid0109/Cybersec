using Cybersec.Domain.Commons;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cybersec.Domain.Entities;
public class Comment:Auditable
{
    [Required]
    public string Content { get; set; }

    // Foreign key to Article
    [ForeignKey("ArticleId")]
    public long ArticleId { get; set; }
    public virtual Article Article { get; set; }

    // Foreign key to User
    [ForeignKey("UserId")]
    public long UserId { get; set; }
    public virtual User User { get; set; }
}
