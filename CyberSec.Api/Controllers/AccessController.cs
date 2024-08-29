using Cybersec.Domain.Enums;
using Cybersec.Service.DTOs.Auth;
using Cybersec.Service.DTOs.Users;
using Cybersec.Service.Interfaces.Auth;
using Cybersec.Service.Interfaces.Users;
using Microsoft.AspNetCore.Mvc;

namespace Cybersec.Controllers
{
    public class AccessController : Controller
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IExistEmail _existEmail;

        public AccessController(IUserService userService, 
                                IAuthService authService,
                                IExistEmail  existEmail)
        {
            _userService = userService;
            _authService = authService;
            _existEmail  = existEmail;
        }

        [HttpGet]
        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginPostModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.AuthenticateAsync(model);

                if (result is not null)
                {
                    await _authService.SaveAccessTokenAsync(result);
                    return RedirectToAction("Index","Home");
                }
                return View(model);
            }

            return View(model);
        }

        [HttpGet]
        public ViewResult ExistEmail()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ExistEmail(EmailModel mail)
        {
            if (ModelState.IsValid)  
            {
                var result = await _existEmail.EmailExistance(mail);

                if (result is EmailExistanceEnum.NotFound)
                    return Redirect("~/Access/Register");

                else if (result is EmailExistanceEnum.NotVerified)
                    return Redirect("~/Access/Verification");

                return Redirect("~/Access/Login");
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
                var user = await _userService.CreateAsync(model);
                return Redirect("~/Access/Verification");
            }
            return View(model);
        }

        [HttpGet]
        public ViewResult Verification()
        {
            return View();
        }

        [HttpPost("Verification")]
        public async Task<IActionResult> Verification(EmailCodeModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _existEmail.VerifyCodeAsync(model.Email,model.Code);

                if(result)
                    return Redirect("~/Access/Login");

                return Redirect("~/Access/ExistEmail");
            }

            return View(model);
        }
    }
}
