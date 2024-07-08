using Cybersec.Service.ViewModels.Article;

namespace Cybersec.Service.ViewModels;
public class HelperViewModel
{
    public IEnumerable<ArticleViewModel> leftSide { get; set; }
    public IEnumerable<ArticleViewModel> rightSide { get; set; }
}
