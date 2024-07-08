using Cybersec.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Cybersec.Service.ViewModels.Article;
public class ArticlePostModel
{
    public string Title { get; set; }
    public Category Category { get; set; }
    public List<IFormFile> Images { get; set; }
    public List<IFormFile> Videos { get; set; }
    public List<string> Texts { get; set; }
    public List<string> Codes { get; set; }
    public string Orders { get; set; }
}
