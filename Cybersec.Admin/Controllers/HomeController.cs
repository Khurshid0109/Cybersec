using Cybersec.Admin.Attributes;
using Cybersec.Service.DTOs.Admins;
using Cybersec.Service.Extentions;
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
        private readonly IUserService _userService;

        public HomeController(IArticleService articleService, 
                              IAdminService adminService, 
                              IUserService userService)
        {
            _articleService = articleService;
            _adminService = adminService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Admins(PaginationParams @params)
        {
            var admins = await _adminService.GetAllAsync(@params);
            return View(admins);
        }

        [HttpGet]
        public async Task<IActionResult> Users(PaginationParams @params, bool deleted = false)
        {
            var users = await _userService.GetAllAsync (@params, deleted);
            return View(users);
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
        public async Task<IActionResult> Settings()
        {
            var id = await _adminService.GetAdminIdFromClaimsAsync();
            var admin = await _adminService.GetAdminByIdAsync(id);

            var mapped = new AdminSettingsModel
            {
                FullName = admin.FullName,
            };
            ViewBag.Id = id;
            return View(mapped);
        }
    }
}
