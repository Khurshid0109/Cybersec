
using Microsoft.AspNetCore.Http;

namespace Cybersec.Service.Helpers;
public class MediaHelper
{
    public static async Task<string> UploadFile(IFormFile file, string mediaType)
    {
        string uniqueFileName = "";
        if (file != null && file.Length > 0)
        {
            string uploadsFolder = Path.Combine(WebHostEnvironmentHelper.WebRootPath, mediaType == "image"? "Images" : "Videos");
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string imageFilePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(imageFilePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
        }

        return uniqueFileName;
    }
}
