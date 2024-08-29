using Cybersec.Admin.Attributes;
using Cybersec.Service.Interfaces.Articles;
using Cybersec.Service.Interfaces.Users;
using Microsoft.AspNetCore.Mvc;

namespace Cybersec.Admin.Controllers
{
    [CustomAuthorize]
    public class HomeController : Controller
    {
        private readonly IArticleService _articleService;
        private readonly IAdminService _adminService;

        public HomeController(IArticleService articleService, IAdminService adminService)
        {
            _articleService = articleService;
            _adminService = adminService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Admins()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Users()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Articles()
        {
            var articles = await _articleService.GetAllArticlesAsync();
            return View(articles);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var id = await _adminService.GetAdminIdFromClaimsAsync();
            var admin =await  _adminService.GetAdminByIdAsync(id);

            return View(admin);
        }

        [HttpGet]
        public IActionResult Settings()
        {
            return View();
        }
    }
}
