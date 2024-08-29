using Cybersec.Api.Attributes;
using Cybersec.Domain.Enums;
using Cybersec.Service.Interfaces.Articles;
using Cybersec.Service.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Cybersec.Controllers
{
    public class HomeController : Controller
    {
        private readonly IArticleService _articleService;

        public HomeController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet, Route("/"), Route("/home"), Route("/home/index/{category}")]
        public async Task<IActionResult> Index(Category? category, [FromQuery] string? q)
        {
            var res = await _articleService.GetAllArticlesAsync();
            HelperViewModel model = new HelperViewModel()
            {
                rightSide = res,
                leftSide = res
            };

            ViewBag.Category = category;

            if (category is not null)
                model.leftSide = res.Where(item => item.Category == category);

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.Trim();
                res = (ICollection<Service.ViewModels.Article.ArticleViewModel>)res.Where(x => x.Title.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(long id)
        {
            var res = await _articleService.GetArticleByIdAsync(id);

            return View(res);
        }

    }
}