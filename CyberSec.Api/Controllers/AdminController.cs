//using Cybersec.Service.Interfaces.News;
//using Microsoft.AspNetCore.Mvc;

//namespace Cybersec.Api.Controllers
//{
//    public class AdminController : Controller
//    {
//        private readonly INewsService _service;

//        public AdminController(INewsService service)
//        {
//            _service = service;
//        }

//        [HttpGet]
//        public async Task<IActionResult> AdminPage()
//        {
//            var res = await _service.RetrieveAllAsync();
//            return View(res);
//        }
//    }
//}
