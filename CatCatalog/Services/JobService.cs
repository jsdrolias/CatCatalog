using CatCatalog.Abstractions;
using CatCatalog.Contexts;
using CatCatalog.Resources;
using Microsoft.EntityFrameworkCore;

namespace CatCatalog.Services;

public class JobService : IJobService
{
    private readonly CatDbContext _context;
    private readonly ICatProcessingService _catProcessingService;

    public JobService(
        CatDbContext context, ICatProcessingService catProcessingService)
    {
        _context = context;
        _catProcessingService = catProcessingService;
    }

    public async Task<JobResponse> Add()
    {
        var now = DateTime.UtcNow;
        var job = new Models.Job
        {
            IsCompleted = false,
            Created = now,
            Updated = now
        };
        _context.Job.Add(job);

        try
        {
            await _context.SaveChangesAsync();
            return new JobResponse
            {
                Id = job.Id,
                IsCompleted = job.IsCompleted
            };
        }
        catch (Exception ex)
        {
         throw new InvalidOperationException(ex.Message);
        }
    }

    public async Task<JobResponse?> GetById(int id)
    {
        var job = await _context.Job
            .SingleOrDefaultAsync(x => x.Id == id);

        var result = job is null
            ? null
            : new JobResponse
            {
                Id = job.Id,
                IsCompleted = job.IsCompleted
            };
        return result;
    }

    public async Task<List<JobResponse>> GetAll()
    {

        var jobs = await _context.Job
            .ToListAsync();

        var result = jobs is null
            ? new List<JobResponse>()
            : jobs.Select(b => new JobResponse { Id = b.Id, IsCompleted = b.IsCompleted }).ToList();
        return result;
    }
}
