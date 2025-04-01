using CatCatalog.Abstractions;
using CatCatalog.Resources;
using Microsoft.AspNetCore.Mvc;

namespace CatCatalog.Controllers
{
    [ApiController]
    [Route("api/cats")]
    public class CatController : ControllerBase
    {
        private readonly ICatProcessingService _catProcessingService;
        private readonly IJobService _jobService;

        public CatController(
            ICatProcessingService catProcessingService,
            IJobService jobService)
        {
            _catProcessingService = catProcessingService;
            _jobService = jobService;
        }


        [HttpPost("fetch")]
        public async Task<IActionResult> Fetch()
        {
            var job = await _jobService.Add();
            return Accepted(job);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CatResponse>> GetCatById(int id)
        {
            var cat = await _catProcessingService.GetById(id);

            if (cat == null)
            {
                return NotFound();
            }
            else
            {
                return cat;
            }
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<CatResponse>>> GetAll(int page = 1, int pageSize = 100, string? tag = "")
        {
            var cat = await _catProcessingService.GetAll(page, pageSize, tag);

            if (cat == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(cat);
            }
        }
    }
}
