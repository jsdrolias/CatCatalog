using CatCatalog.Abstractions;
using CatCatalog.DTO;
using System.Text.Json;

namespace CatCatalog.Services;

public class CatWebClientService : ICatWebClientService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public CatWebClientService(
        HttpClient httpClient, 
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["CatApiKey"] ?? throw new ArgumentNullException("API Key is missing");
    }

    public async Task<List<CatFromWebDTO>> FetchCatImagesAsync(int limit)
    {
        _httpClient.DefaultRequestHeaders.Clear();
        _httpClient.DefaultRequestHeaders.Add("x-api-key", _apiKey);

        string requestUri = $"https://api.thecatapi.com/v1/images/search?limit={limit}&has_breeds=true";

        HttpResponseMessage response = await _httpClient.GetAsync(requestUri);
        response.EnsureSuccessStatusCode();

        string jsonResponse = await response.Content.ReadAsStringAsync();
        var catImages = JsonSerializer.Deserialize<List<CatFromWebDTO>>(jsonResponse) ?? new List<CatFromWebDTO>();
        return catImages;
    }
}
