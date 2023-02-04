using Azure.Storage.Blobs;
using DataLibrary.Models;

namespace DataLibrary.DbServices;

public class BlobService
{
    private readonly BlobServiceClient _blobServiceClient;
    public BlobService(BlobServiceClient blobServiceClient)
    {
        _blobServiceClient = blobServiceClient;
    }
    public Task<GroundRentPdfModel> CreateOrUpdateGroundRentPdf(GroundRentPdfModel groundRentPdfModel)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient("baltimorecitygroundrentpdfs");
    }
}
