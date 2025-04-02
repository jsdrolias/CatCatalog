using CatCatalog.Abstractions;
using CatCatalog.Resources;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace CatCatalog.Controllers
{
    /// <summary>
    /// Cat endponits.
    /// </summary>
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
        [SwaggerOperation(Summary = "Fetch cats", Description = "Triggers job to fetch 25 cats.")]
        [SwaggerResponse(202)]
        public async Task<IActionResult> Fetch()
        {
            var job = await _jobService.Add();
            return Accepted(job);
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get cat", Description = "Get a cat by id.")]
        [SwaggerResponse(200)]
        [SwaggerResponse(404)]
        public async Task<ActionResult<CatResponse>> GetCatById(
            [SwaggerParameter("Cat id")][FromRoute] int id)
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
        [SwaggerOperation(Summary = "Get all cats", Description = "Retrieves a list of all available cats.")]
        [SwaggerResponse(200)]
        public async Task<ActionResult<IEnumerable<CatResponse>>> GetAll(
            [SwaggerParameter("Page number")][FromQuery] int? page,
            [SwaggerParameter("Page size")][FromQuery] int? pageSize,
            [SwaggerParameter("Cat tag filter (case insensitive)")][FromQuery] string? tag)
        {
            try
            {
                var cats = await _catProcessingService.GetAll(page, pageSize, tag);
                return Ok(cats);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
