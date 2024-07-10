using Cybersec.Domain.Enums;

namespace Cybersec.Service.ViewModels.Article;
public class ArticleViewModel
{
    public long Id { get; set; }
    public string Title { get; set; }
    public Category Category { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }    
    public List<ContentBlockViewModel> Blocks { get; set; }
}
