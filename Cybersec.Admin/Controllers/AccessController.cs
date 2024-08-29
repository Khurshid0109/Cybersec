using Cybersec.Service.DTOs.Admins;
using Cybersec.Service.Interfaces.Users;
using Microsoft.AspNetCore.Mvc;

namespace Cybersec.Admin.Controllers
{
    public class AccessController : Controller
    {
        private readonly IAdminService _adminService;

        public AccessController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (ModelState.IsValid)
            {
                var success = await _adminService.LoginAsync(loginDto);
                if (success)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(loginDto);
        }

    }
}
