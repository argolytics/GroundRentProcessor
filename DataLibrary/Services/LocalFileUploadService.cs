using Microsoft.AspNetCore.Http;

namespace DataLibrary.Services;
public class LocalFileUploadService : IFileUploadService
{
    public async Task<string> UploadFile(IFormFile file)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "FilesToParse", file.FileName);
        using var fileStream = new FileStream(filePath, FileMode.Create);
        await file.CopyToAsync(fileStream);
        return filePath;
    }
}
