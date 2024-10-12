namespace Cybersec.Service.DTOs.Comment;
public class CommentPutModel
{
    public string Content { get; set; }
    public long ArticleId { get; set; }
    public long UserId { get; set; }
}