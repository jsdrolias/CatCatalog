using CatCatalog.Abstractions;
using CatCatalog.Resources;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CatCatalog.Controllers
{
    [ApiController]
    [Route("api/jobs")]
    public class JobController : ControllerBase
    {
        private readonly IJobService _jobService;

        public JobController(IJobService jobService)
        {
            _jobService = jobService;
        }


        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get job", Description = "Get a job by id.")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404)]
        public async Task<ActionResult<JobResponse>> GetJobById(
            [SwaggerParameter("Job id")][FromQuery] int id)
        {
            var job = await _jobService.GetById(id);

            if (job is null)
            {
                return NotFound();
            }
            else
            {
                return Ok(job);
            }
        }

        [HttpGet()]
        [SwaggerOperation(Summary = "Get all jobs", Description = "Retrieves a list of all jobs.")]
        [SwaggerResponse(200)]
        public async Task<ActionResult<JobResponse>> GetJobs()
        {
            var jobs = await _jobService.GetAll();
            return Ok(jobs);
        }
    }
}
