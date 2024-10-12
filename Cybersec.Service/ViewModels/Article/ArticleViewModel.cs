using Cybersec.Domain.Entities;
using Cybersec.Domain.Enums;
using Cybersec.Service.DTOs.Like;

namespace Cybersec.Service.ViewModels.Article;
public class ArticleViewModel
{
    public long Id { get; set; }
    public string Title { get; set; }
    public Category Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public int ViewCount { get; set; }
    public  ICollection<LikeViewModel> Likes { get; set; }
    public  ICollection<Comment> Comments { get; set; }
    public List<ContentBlockViewModel> Blocks { get; set; }
}
