using Microsoft.AspNetCore.Http;

namespace DataLibrary.Services;
public interface IFileUploadService
{
    Task<string> UploadFile(IFormFile file);
}
