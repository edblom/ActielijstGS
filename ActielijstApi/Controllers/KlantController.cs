using ActielijstApi.Dtos;
using ActieLijstAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ActieLijstAPI.Controllers
{
    [Route("api/klant")]
    [ApiController]
    public class KlantController : ControllerBase
    {
        private readonly KlantService _klantService;

        public KlantController(KlantService klantService)
        {
            _klantService = klantService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<KlantDTO>> Get(int id)
        {
            var klant = await _klantService.GetByIdAsync(id);
            return Ok(klant);
        }
    }
}