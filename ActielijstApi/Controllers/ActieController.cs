using Microsoft.AspNetCore.Mvc;
using ActielijstApi.Models;
using ActielijstApi.Services;
using KlantBaseShare.Dtos;
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

        // GET: api/acties
        [HttpGet]
        public async Task<ActionResult> GetAllActies(
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? status = null,
            [FromQuery] int? werknemerId = null,
            [FromQuery] string? actieSoortId = null,
            [FromQuery] int? priorityId = null,
            [FromQuery] int? werknId = null, // New parameter for filtering by WerknId
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 0,
            [FromQuery] string? sortBy = null,
            [FromQuery] string? sortDirection = "asc")
        {
            // Backwards compatibility: return List<Actie> if no parameters
            if (string.IsNullOrWhiteSpace(searchTerm) && string.IsNullOrWhiteSpace(status) && !werknemerId.HasValue &&
                string.IsNullOrWhiteSpace(actieSoortId) && !priorityId.HasValue && !werknId.HasValue && page == 1 && pageSize == 0 && string.IsNullOrWhiteSpace(sortBy))
            {
                var acties = await _actieService.GetAllActiesAsync();
                return Ok(acties);
            }

            // Use filtered method for server-side filtering, paging, and sorting
            var response = await _actieService.GetFilteredActiesAsync(searchTerm, status, werknemerId, actieSoortId, priorityId, werknId, page, pageSize, sortBy, sortDirection);
            return Ok(response);
        }

        // GET: api/acties/user/{userId}/{filterType}
        [HttpGet("user/{userId}/{filterType}")]
        public async Task<ActionResult<List<Actie>>> GetActiesByUser(int userId, string filterType)
        {
            var acties = await _actieService.GetActiesByUserAsync(userId, filterType);
            return Ok(acties);
        }

        // GET: api/acties/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Actie>> GetActie(int id)
        {
            var actie = await _actieService.GetActieByIdAsync(id);
            if (actie == null) return NotFound();
            return Ok(actie);
        }

        // POST: api/acties
        [HttpPost]
        public async Task<ActionResult<Actie>> PostActie(Actie actie)
        {
            var createdActie = await _actieService.CreateActieAsync(actie);
            return CreatedAtAction(nameof(GetActie), new { id = createdActie.FldMid }, createdActie);
        }

        // PUT: api/acties/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActie(int id, Actie actie)
        {
            if (id != actie.FldMid) return BadRequest();
            var success = await _actieService.UpdateActieAsync(id, actie);
            if (!success) return NotFound();
            return NoContent();
        }

        // PATCH: api/acties/{id}
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchActie(int id, [FromBody] PatchActieDto updates)
        {
            var success = await _actieService.PatchActieAsync(id, updates);
            if (!success) return NotFound();
            return NoContent();
        }

        // DELETE: api/acties/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActie(int id)
        {
            var success = await _actieService.DeleteActieAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}