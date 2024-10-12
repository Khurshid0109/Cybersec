using Cybersec.Service.DTOs.Users;
using Cybersec.Service.ViewModels.Article;

namespace Cybersec.Service.DTOs.Comment;
public class CommentViewModel
{
    public long Id { get; set; }
    public string Content { get; set; }
    public long ArticleId { get; set; }
    public  ArticleViewModel Article { get; set; }

    public long UserId { get; set; }
    public  UserViewModel User { get; set; }
}
