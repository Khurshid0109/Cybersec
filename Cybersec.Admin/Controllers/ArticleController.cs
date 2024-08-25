using Microsoft.AspNetCore.Mvc;
using Cybersec.Service.Interfaces.Articles;
using Cybersec.Service.ViewModels.Article;

namespace Cybersec.Admin.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ArticlePostModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _articleService.CreateArticleAsync(model);
                return RedirectToAction(nameof(Details),new {Id = result.Id});
            }
            return BadRequest(ModelState);
        }

        public async Task<IActionResult> Details(long id)
        {
            var article =await _articleService.GetArticleByIdAsync(id);

            return View(article);
        }
    }
}
