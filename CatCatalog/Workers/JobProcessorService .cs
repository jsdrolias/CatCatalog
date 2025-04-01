using CatCatalog.Abstractions;
using CatCatalog.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CatCatalog.Workers;

public class JobProcessorService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<JobProcessorService> _logger;

    public JobProcessorService(IServiceProvider serviceProvider, ILogger<JobProcessorService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Job Processor Service is starting...");

        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<CatDbContext>();
                var catProcessingService = scope.ServiceProvider.GetRequiredService<ICatProcessingService>();

                var pendingJobs = await dbContext.Job
                    .Where(j => !j.IsCompleted)
                    .ToListAsync(stoppingToken);

                foreach (var job in pendingJobs)
                {
                    try
                    {
                        await catProcessingService.FetchImages();

                        job.IsCompleted = true;
                        dbContext.Job.Update(job);
                        await dbContext.SaveChangesAsync(stoppingToken);

                        _logger.LogInformation($"Job {job.Id} completed.");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error processing job {job.Id}: {ex.Message}");
                    }
                }
            }

            // Wait before checking for new jobs
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
        }
    }
}
