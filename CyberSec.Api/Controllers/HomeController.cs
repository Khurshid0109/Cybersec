using Cybersec.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Cybersec.Service.Interfaces.News;
using Cybersec.Service.ViewModels.News;
using Cybersec.Service.ViewModels;

namespace Cybersec.Controllers
{
    public class HomeController : Controller
    {
    
        private readonly INewsService _newsService;
        public HomeController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet,Route("/"),Route("/home"),Route("/home/index/{category}")]
        public async Task<IActionResult> Index(Categories? category, [FromQuery] string? q)
        {            
            var res= await _newsService.RetrieveAllAsync();
            HelperViewModel model = new HelperViewModel()
            {
                rightSide = res,
                leftSide=res
            };

            ViewBag.Category = category;

            if (category is not null)
                model.leftSide = res.Where(item => item.Category == category);

            if (!string.IsNullOrWhiteSpace(q))
            {
                q=q.Trim();
                res = res.Where(x => x.Title.Contains(q, StringComparison.OrdinalIgnoreCase) 
                    || x.Description.Contains(q, StringComparison.OrdinalIgnoreCase));
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var res = await _newsService.RetrieveByIdAsync(id);

            return View(res);
        }


        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewsPostModel model)
        {
            if (ModelState.IsValid)
            {
                var news = await _newsService.CreateAsync(model);

                return RedirectToAction("details", new { id = news.Id });
            }
            return View();
        }

    }
}