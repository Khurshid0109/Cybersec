using Cybersec.Domain.Commons;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cybersec.Domain.Entities;
public class Like:Auditable
{
    [ForeignKey("ArticleId")]
    public long ArticleId { get; set; }

    public virtual Article Article { get; set; }

    [ForeignKey("UserId")]
    public long UserId { get; set; }

    public  User User { get; set; }
}