using Cybersec.Domain.Enums;
using Cybersec.Domain.Commons;

namespace Cybersec.Domain.Entities;
public class Article:Auditable
{
    public string Title { get; set; }
    public Category Category { get; set; }
    public List<ContentBlock>? Blocks { get; set; } 
}
