using Cybersec.Domain.Enums;

namespace Cybersec.Domain.Commons;
public class Auditable
{
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Status Status { get; set; }
}
