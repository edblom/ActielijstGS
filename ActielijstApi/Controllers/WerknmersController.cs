using Microsoft.AspNetCore.Mvc;
using ActielijstApi.Models;
using Microsoft.EntityFrameworkCore;
using ActielijstApi.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace ActielijstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WerknemersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public WerknemersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Werknemer>>> GetWerknemers()
        {
            try
            {
                var workers = await _context.Werknemers.ToListAsync();
                return Ok(workers);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("actueel")]
        public async Task<ActionResult<IEnumerable<Werknemer>>> GetActueleWerknemers()
        {
            try
            {
                var currentDate = new DateTime(2025, 4, 7); // Huidige datum (7 april 2025)
                var actueleWerknemers = await _context.Werknemers
                    .Where(w => w.FldDatumUitDienst == null || w.FldDatumUitDienst > currentDate)
                    .OrderBy(w => w.Voornaam)
                    .ToListAsync();

                return Ok(actueleWerknemers);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}