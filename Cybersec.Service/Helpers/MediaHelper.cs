using Microsoft.AspNetCore.Http;

namespace Cybersec.Service.Helpers
{
    public static class MediaHelper
    {
        public static async Task<string> UploadFile(IFormFile file, string mediaType)
        {
            string mediaRootPath = GetMediaRootPath();
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

        private static string GetMediaRootPath()
        {
            // Assuming the SharedResources project is at the same level as other projects
            var baseDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).FullName;
            return Path.Combine(baseDirectory, "Cybersec.SharedResources", "Shared");
        }
    }
}
