using ActielijstApi.Data;
using ActielijstApi.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ActieLijstAPI.Services
{
    public class KlantService
    {
        private readonly ApplicationDbContext _context;

        public KlantService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<KlantDTO> GetByIdAsync(int id)
        {
            var klant = await _context.Adressen
                .Where(a => a.Id == id)
                .FirstOrDefaultAsync();
            return klant == null
                ? new KlantDTO
                {
                    Id = 0,
                    Naam = "Onbekend",
                    Plaats = "",
                    Zoekcode = ""
                }
                : new KlantDTO
                {
                    Id = klant.Id,
                    Naam = klant.Bedrijf,
                    Plaats = klant.Plaats,
                    Zoekcode = klant.ZOEKCODE
                };
        }
        public async Task<List<KlantDTO>> SearchAsync(string term, int limit = 50)
        {
            var query = _context.Adressen.Where(k => k.ZOEKCODE.Contains(term) || k.Bedrijf.Contains(term));
            return await query
                .Select(k => new KlantDTO
                {
                    Id = k.Id,
                    Zoekcode = k.ZOEKCODE,
                    Naam = k.Bedrijf,
                    Plaats = k.Plaats
                })
                .Take(limit)
                .ToListAsync();
        }
    }
}