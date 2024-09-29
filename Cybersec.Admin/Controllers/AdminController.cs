using Microsoft.AspNetCore.Mvc;
using Cybersec.Service.DTOs.Admins;
using Cybersec.Service.Interfaces.Users;
 
namespace Cybersec.Admin.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public IActionResult AddNewAdmin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewAdmin( AdminPostModel model)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var admin = await _adminService.AddAdminAsync(model);
                return RedirectToAction("Admins", "Home");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditAdminData(long id)
        {
           var admin = await _adminService.GetAdminByIdAsync(id);
            var mapped = new AdminPutModel
            {
                FullName = admin.FullName,
                Email = admin.Email,
                Status = admin.Status
            };

            ViewBag.Id = id;
            return View(mapped);
        }

        [HttpPost]
        public async Task<IActionResult> EditAdminDAta(long id,AdminPutModel model)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var admin = await _adminService.UpdateAdminAsync(id,model);
                return RedirectToAction("Admins", "Home");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}
