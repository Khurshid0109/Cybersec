using Microsoft.AspNetCore.Mvc;
using Cybersec.Service.ViewModels.Users;

namespace Cybersec.Controllers
{
    public class AccessController : Controller
    {
        [HttpGet]
        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
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
    }
}
