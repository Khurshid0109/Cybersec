namespace Cybersec.Domain.Entities;
public abstract class ContentBlock
{
    public long Id { get; set; }
    public long Order { get; set; }
    public long ArticleId { get; set; }
    public Article Article { get; set; }
}
