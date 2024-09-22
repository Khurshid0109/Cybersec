using Microsoft.AspNetCore.Mvc;

namespace Cybersec.Api.Controllers
{
    public class MediaController : Controller
    {
        private readonly string _sharedMediaPath;
        public MediaController(IConfiguration configuration)
        {
            _sharedMediaPath = configuration["SharedMedia:MediaPath"];
        }

        [HttpGet]
        public IActionResult GetImage([FromQuery] string fileName)
        {
            var filePath = Path.Combine(_sharedMediaPath, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "image/jpeg");
        }

        [HttpGet]
        public IActionResult GetVideo([FromQuery] string fileName)
        {
            var filePath = Path.Combine(_sharedMediaPath, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "video/mp4");
        }
    }
}
