namespace CatCatalog.Abstractions;

public interface IBlobStorageService
{
    public Task<string> DownloadAndUploadImageAsync(string imageUrl, string blobName);
}
