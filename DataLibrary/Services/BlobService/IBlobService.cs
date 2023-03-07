using OpenQA.Selenium;

namespace DataLibrary.Services.BlobService;

public interface IBlobService
{
    public Task<bool> UploadBlob(string blobName, PrintDocument printDocument, string containerName);
}
