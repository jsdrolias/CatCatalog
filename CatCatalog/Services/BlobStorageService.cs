using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CatCatalog.Options;
using Microsoft.Extensions.Options;

namespace CatCatalog.Services;

public class BlobStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly HttpClient _httpClient;
    private readonly BlobStorageOptions _options;

    public BlobStorageService(
        HttpClient httpClient,
        IOptions<BlobStorageOptions> options)
    {
        _options = options.Value;
        _blobServiceClient = new BlobServiceClient(_options.ConnectionString);
        _httpClient = httpClient;
    }

    public async Task<string> DownloadAndUploadImageAsync(string imageUrl, string containerName, string blobName)
    {
        // Ensure container exists
        var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);

        // Download the image from the given URL
        byte[] imageBytes = await _httpClient.GetByteArrayAsync(imageUrl);

        // Upload image to blob storage
        var blobClient = containerClient.GetBlobClient(blobName);
        using var stream = new MemoryStream(imageBytes);
        await blobClient.UploadAsync(stream, true);

        return blobClient.Uri.ToString();
    }
}
