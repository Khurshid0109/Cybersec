using Cybersec.Service.ViewModels.News;

namespace Cybersec.Service.Interfaces.News;
public interface INewsService
{
    Task<bool> RemoveAsync(int id);
    Task<NewsModel> RetrieveByIdAsync(int id);
    Task<IEnumerable<NewsModel>> RetrieveAllAsync();
    Task<NewsModel> CreateAsync(NewsPostModel dto);
    Task<NewsModel> ModifyAsync(int id, NewsPutModel dto);
}
