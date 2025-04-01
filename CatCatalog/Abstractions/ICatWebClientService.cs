using CatCatalog.DTO;

namespace CatCatalog.Abstractions;

public interface ICatWebClientService
{
    Task<List<CatFromWebDTO>> FetchCatImagesAsync(int limit);
}
