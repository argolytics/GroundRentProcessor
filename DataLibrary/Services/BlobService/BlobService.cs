using DataLibrary.Settings;
using DataLibrary.Models;
using Azure.Storage.Blobs;
using System.Text;

namespace DataLibrary.Services.BlobService;

public class BlobService : IBlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly BlobSettings _blobSettings;
    private readonly string _blobContainerClient;

	public BlobService(BlobServiceClient blobServiceClient, BlobSettings blobSettings)
	{
        _blobServiceClient = blobServiceClient;
        _blobSettings = blobSettings;
        _blobContainerClient= _blobSettings.BlobContainerClient;
    }
    public async Task<BlobInfo> ReadBlob(string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerClient);
        var blobClient = containerClient.GetBlobClient(blobName);
        var blobDownloadInfo = await blobClient.DownloadAsync();
        return new BlobInfo(blobDownloadInfo.Value.Content, blobDownloadInfo.Value.ContentType);
    }
    public async Task<IEnumerable<string>> ReadAllBlob()
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerClient);
        var blobItems = new List<string>();
        await foreach(var blobItem in containerClient.GetBlobsAsync())
        {
            blobItems.Add(blobItem.Name);
        }
        return blobItems;
    }
    public async Task Upload(string content, string fileName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerClient);
        var blobClient = containerClient.GetBlobClient(fileName);
        var bytes = Encoding.UTF8.GetBytes(content);
        using var memoryStream = new MemoryStream(bytes);
        await blobClient.UploadAsync(memoryStream);
    }
    public async Task Delete(string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerClient);
        var blobClient = containerClient.GetBlobClient(blobName);
        await blobClient.DeleteIfExistsAsync();
    }
}
