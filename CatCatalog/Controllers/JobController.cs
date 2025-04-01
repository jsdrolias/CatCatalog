using CatCatalog.Abstractions;
using CatCatalog.Resources;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<JobResponse>> GetJobById(int id)
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
        public async Task<ActionResult<JobResponse>> GetJobs()
        {
            var jobs = await _jobService.GetAll();
            return Ok(jobs);
        }
    }
}
