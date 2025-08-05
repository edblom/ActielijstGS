using KlantBaseShare.Dtos;
using ActieLijstAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActieLijstAPI.Controllers
{
    [Route("api/opdrachten")]
    [ApiController]
    public class OpdrachtController : ControllerBase
    {
        private readonly OpdrachtService _opdrachtService;

        public OpdrachtController(OpdrachtService opdrachtService)
        {
            _opdrachtService = opdrachtService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OpdrachtDTO>> Get(int id)
        {
            var opdracht = await _opdrachtService.GetByIdAsync(id);
            return Ok(opdracht);
        }

        [HttpGet("search")]
        public async Task<ActionResult<List<OpdrachtDTO>>> Search(string term, int limit = 50)
        {
            var results = await _opdrachtService.SearchAsync(term, limit);
            return Ok(results);
        }
    }
}