using KlantBaseShare.Dtos;
using ActielijstApi.Models;
using ActielijstApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActielijstApi.Controllers
{
    [ApiController]
    [Route("api/acties")]
    public class ActieController : ControllerBase
    {
        private readonly IActieService _actieService;

        public ActieController(IActieService actieService)
        {
            _actieService = actieService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Actie>>> GetAllActies()
        {
            var acties = await _actieService.GetAllActiesAsync();
            return Ok(acties);
        }

        [HttpGet("user/{userId}/{filterType}")]
        public async Task<ActionResult<List<Actie>>> GetActiesByUser(int userId, string filterType)
        {
            var acties = await _actieService.GetActiesByUserAsync(userId, filterType);
            return Ok(acties);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Actie>> GetActie(int id)
        {
            var actie = await _actieService.GetActieByIdAsync(id);
            if (actie == null) return NotFound();
            return Ok(actie);
        }

        [HttpPost]
        public async Task<ActionResult<Actie>> PostActie(Actie actie)
        {
            var createdActie = await _actieService.CreateActieAsync(actie);
            return CreatedAtAction(nameof(GetActie), new { id = createdActie.FldMid }, createdActie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutActie(int id, Actie actie)
        {
            if (id != actie.FldMid) return BadRequest();
            var success = await _actieService.UpdateActieAsync(id, actie);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchActie(int id, [FromBody] PatchActieDto updates)
        {
            var success = await _actieService.PatchActieAsync(id, updates);
            if (!success) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActie(int id)
        {
            var success = await _actieService.DeleteActieAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}