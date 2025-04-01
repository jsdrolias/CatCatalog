using CatCatalog.Resources;

namespace CatCatalog.Abstractions;

public interface ICatProcessingService
{
    public Task<CatResponse?> GetById(int id);
    public Task<IEnumerable<CatResponse>> GetAll(int? page, int? pageSize, string? tag);
    public Task FetchImages();
}
