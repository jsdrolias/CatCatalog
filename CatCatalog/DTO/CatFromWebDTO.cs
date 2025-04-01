using System.Text.Json.Serialization;

namespace CatCatalog.DTO;

public class CatFromWebDTO
{
    [JsonPropertyName("id")]
    public string ImageId { get; set; }

    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }

    [JsonPropertyName("url")]
    public string ImageUrl { get; set; }

    [JsonPropertyName("breeds")]

    public List<BreedFromWebDTO> Breeds { get; set; } = new();
}
