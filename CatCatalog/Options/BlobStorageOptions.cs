namespace CatCatalog.Options;

public class BlobStorageOptions
{
    public const string Section = "BlobStorage";
    public string ConnectionString { get; set; } = string.Empty;
    public string ContainerName { get; set; } = string.Empty;
}
