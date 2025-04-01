using CatCatalog.Resources;

namespace CatCatalog.Abstractions;

public interface IJobService
{
    public Task<JobResponse> Add();

    public Task<JobResponse?> GetById(int id);

    public Task<List<JobResponse>> GetAll();
}
