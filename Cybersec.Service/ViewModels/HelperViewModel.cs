
using Cybersec.Service.ViewModels.News;

namespace Cybersec.Service.ViewModels;
public class HelperViewModel
{
    public IEnumerable<NewsModel> leftSide { get; set; }
    public IEnumerable<NewsModel> rightSide { get; set; }
}
