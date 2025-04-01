using System.Text.Json.Serialization;

namespace CatCatalog.DTO;

public class BreedFromWebDTO
{
    
    [JsonPropertyName("temperament")]
    public string Temperament { get; set; }
}
