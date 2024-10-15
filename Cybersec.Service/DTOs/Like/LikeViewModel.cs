using Cybersec.Domain.Entities;
using Cybersec.Domain.Enums;
using Cybersec.Service.DTOs.Users;

namespace Cybersec.Service.DTOs.Like;
public class LikeViewModel
{
    public long Id { get; set; }

    public long ArticleId { get; set; }

    public  Article Article { get; set; }

    public long UserId { get; set; }

    public UserViewModel User { get; set; }
    public Status Status { get; set; }
}