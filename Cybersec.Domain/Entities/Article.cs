using Cybersec.Domain.Enums;

namespace Cybersec.Domain.Entities;
public class Article
{
    public long Id { get; set; }
    public string Title { get; set; }
    public Category Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<ContentBlock> Blocks { get; set; } = new List<ContentBlock>();
}
