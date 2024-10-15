using Cybersec.Service.ViewModels.Article;

namespace Cybersec.Api.ViewModels;
public class ArticleDetailsViewModel
{
    public ArticleViewModel ArticleViewModel { get; set; }
    public bool isLiked { get; set; }
    public int LikesCount { get; set; }
}
