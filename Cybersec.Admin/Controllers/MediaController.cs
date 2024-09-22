using Microsoft.AspNetCore.Mvc;

namespace Cybersec.Admin.Controllers
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

        [HttpDelete]
        public IActionResult DeleteImage([FromQuery] string fileName)
        {
            var filePath = Path.Combine(_sharedMediaPath, fileName);

            // Check if the file exists
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("The file does not exist.");
            }

            try
            {
                // Delete the file
                System.IO.File.Delete(filePath);
                return Ok("Image deleted successfully.");
            }
            catch (Exception ex)
            {
                // Handle any exceptions that might occur during deletion
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }
}
