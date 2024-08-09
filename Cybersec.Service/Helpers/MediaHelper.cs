using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Cybersec.Service.Helpers
{
    public static class MediaHelper
    {
        public static IConfiguration Configuration { get; set; }

        public static async Task<string> UploadFile(IFormFile file, string mediaType)
        {
            string mediaRootPath = Configuration["MediaSettings:MediaRootPath"];
            string uploadsFolder = Path.Combine(mediaRootPath, mediaType == "image" ? "Images" : "Videos");

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return uniqueFileName;
        }
    }
}
