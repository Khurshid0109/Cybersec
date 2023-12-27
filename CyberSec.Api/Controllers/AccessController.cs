using Microsoft.AspNetCore.Mvc;
using Cybersec.Service.ViewModels.Users;
using Cybersec.Service.Interfaces.Users;

namespace Cybersec.Controllers
{
    public class AccessController : Controller
    {
        private readonly IUserService _userService;

        public AccessController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public ViewResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model) 
        {
            if(ModelState.IsValid)
            {

            }
            return View();
        }

        [HttpGet]
        public ViewResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserPostModel model)
        {
            if (ModelState.IsValid)
            {
             var user = _userService.CreateAsync(model);
            }
            return View();
        }
    }
}
