using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CoolGuysBackend.UseCases._contracts;

namespace CoolGuysBackend.Helpers;

public class BlobStorageHelper : IBlobStorageHelper
{
    private readonly string connectionString;

    public BlobStorageHelper(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public async Task<string> AddImage(string containerName, int id, IFormFile image, bool overwrite = true)
    {
        try
        {
            var blobClient = new BlobServiceClient(connectionString);
            var container = blobClient.GetBlobContainerClient(containerName);
            await container.CreateIfNotExistsAsync();
            await container.SetAccessPolicyAsync(PublicAccessType.Blob);
            var blob = container.GetBlobClient($"{id}.jpg");
            var stream = image.OpenReadStream();
            await blob.UploadAsync(stream, overwrite);
            return blob.Uri.AbsoluteUri;
        }
        catch (Exception err)
        {
            throw err;
        }
    }
}