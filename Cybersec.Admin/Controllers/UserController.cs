using Cybersec.Service.DTOs.Users;
using Cybersec.Service.Interfaces.Users;
using Microsoft.AspNetCore.Mvc;

namespace Cybersec.Admin.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult AddNewUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewUser(UserPostModel model)
        {
            try
            {
                var user = await _userService.CreateByAdminAsync(model);

                return RedirectToAction("Users", "Home");
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUserData(long id)
        {
            var user = await _userService.GetByIdAsync(id);
            var mapped = new UserByAdminPutModel
            {
               FirstName = user.FirstName,
               LastName = user.LastName,
               Email = user.Email,
               Status = user.Status
            };

            ViewBag.Id = id;
            return View(mapped);
        }

        [HttpPost]
        public async Task<IActionResult> EditUserData(long id, UserByAdminPutModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var admin = await _userService.UpdateByAdminAsync(id, model);
                return RedirectToAction("Users", "Home");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
