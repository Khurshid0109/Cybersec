using Cybersec.Api.Attributes;
using Cybersec.Api.Extentions;
using Cybersec.Api.ViewModels;
using Cybersec.Domain.Enums;
using Cybersec.Service.DTOs.Like;
using Cybersec.Service.Interfaces.Articles;
using Cybersec.Service.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Cybersec.Controllers
{
    public class HomeController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly ILikeService _likeService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(IArticleService articleService,
                              IHttpContextAccessor httpContextAccessor,
                              ILikeService likeService)
        {
            _articleService = articleService;
            _httpContextAccessor = httpContextAccessor;
            _likeService = likeService;
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
        [CustomAuthorize]
        public async Task<IActionResult> Details(long id)
        {
            var res = await _articleService.GetArticleByIdAsync(id);

            var user = HttpContextExtention.GetUser(_httpContextAccessor.HttpContext);


            var model = new ArticleDetailsViewModel
            {
                ArticleViewModel = res
            };

            model.isLiked = res.Likes.Any(x => x.UserId == user.Id && x.Status == Status.Active);
            model.LikesCount = res.Likes.Where(x => x.Status == Status.Active).Count();
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LikedArticle(long articleId,bool isLiked)
        {
            var user = HttpContextExtention.GetUser(_httpContextAccessor.HttpContext);

            var model = new LikePostModel
            {
                ArticleId = articleId,
                UserId = user.Id
            };
            if(!isLiked)
            await _likeService.CreateLikeAsync(model);
            else
                await _likeService.DeleteLikeAsync(articleId,user.Id);
            return RedirectToAction("Details","Home",new {id = articleId});
        }

    }
}