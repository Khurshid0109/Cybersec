using Microsoft.AspNetCore.Http;

namespace Cybersec.Service.Helpers
{
    public static class MediaHelper
    {
        public static async Task<string> UploadFile(IFormFile file, string mediaType)
        {
            string uniqueFileName = "";
            if (file != null && file.Length > 0)
            {
                // Path to the SharedMedia folder in the root solution directory
                string sharedFolderPath = Path.Combine(AppContext.BaseDirectory, "..", "SharedMedia");
                string uploadsFolder = Path.Combine(sharedFolderPath, mediaType == "image" ? "Images" : "Videos");

                // Create directory if it does not exist
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}
