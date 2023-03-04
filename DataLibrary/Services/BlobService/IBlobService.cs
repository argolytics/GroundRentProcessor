using DataLibrary.Models;

namespace DataLibrary.Services.BlobService;

public interface IBlobService
{
    public Task<BlobInfo> ReadBlob(string blobName);
    public Task<IEnumerable<string>> ReadAllBlob();
    public Task Upload(string content, string fileName);
    public Task Delete(string blobName);
}
