using Microsoft.AspNetCore.Mvc;

namespace Cybersec.Api.Controllers;

public class ErrorHandlerController : Controller
{
    [Route("/ErrorHandler/GlobalError")]
    public IActionResult GlobalError(int? statusCode = null)
    {
        if (statusCode.HasValue)
        {
            ViewData["StatusCode"] = statusCode.Value;
        }

        return View();
    }
}
