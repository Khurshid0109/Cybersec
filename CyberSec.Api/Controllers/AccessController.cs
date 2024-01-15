using Cybersec.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Cybersec.Service.Interfaces.Auth;
using Cybersec.Service.ViewModels.Users;
using Cybersec.Service.Interfaces.Users;

namespace Cybersec.Controllers
{
    public class AccessController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserAuthentication _authService;

        public AccessController(IUserService userService, IUserAuthentication authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpGet]
        public ViewResult Login()
        {
            // Retrieve the email from TempData
            string? userEmail = TempData["Email"] as string;

            // Pass the email to the view
            ViewBag.Email = userEmail;

            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {

            }

            return View();
        }

        [HttpGet]
        public ViewResult ExistEmail()
        {
            return View();
        }

        [HttpPost("ExistEmail")]
        public async Task<IActionResult> ExistEmail(string email)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                TempData["Email"] = email;

                var result = await _authService.CheckEmail(email);

                if (result is EmailExistance.NotFound)
                    return Redirect("~/Access/Register");

                else if (result is EmailExistance.NotVerified)
                    return Redirect("~/Access/Verification");

                return Redirect("~/Access/Login");
            }

            return View();
        }

        [HttpGet]
        public ViewResult Register()
        {
            // Retrieve the email from TempData
            string? userEmail = TempData["Email"] as string;

            // Pass the email to the view
            ViewBag.Email = userEmail;

            return View();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserPostModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userService.CreateAsync(model);
                return Redirect("~/Access/Verification");
            }
            return View();
        }

        [HttpGet]
        public ViewResult Verification()
        {
            // Retrieve the email from TempData
            string? userEmail = TempData["Email"] as string;

            // Pass the email to the view
            ViewBag.Email = userEmail;

            return View();
        }

        //[HttpPost("Verification")]
        //public async Task<IActionResult> Verification(User)
        //{
        //    if (ModelState.IsValid)
        //    {

        //    }

        //    return View();
        //}
    }
}
