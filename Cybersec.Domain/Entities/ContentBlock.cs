using Cybersec.Domain.Commons;

namespace Cybersec.Domain.Entities;
public abstract class ContentBlock:Auditable
{
    public long Order { get; set; }
    public long ArticleId { get; set; }
    public Article Article { get; set; }
}
