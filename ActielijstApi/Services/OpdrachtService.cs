using ActielijstApi.Data;
using ActielijstApi.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class OpdrachtService
{
    private readonly ApplicationDbContext _context;

    public OpdrachtService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OpdrachtDTO> GetByIdAsync(int id)
    {
        var opdracht = await _context.ProjectAssignments
            .Where(o => o.Id == id)
            .FirstOrDefaultAsync();
        return opdracht == null
            ? new OpdrachtDTO
            {
                Id =0,
                OpdrachtNummer = "",
                SoortOpdracht = 0,
                Omschrijving =""
            }
            : new OpdrachtDTO
            {
                Id = opdracht.Id,
                OpdrachtNummer = opdracht.FldOpdrachtStr,
                SoortOpdracht = opdracht.FldSoort,
                Omschrijving = opdracht.FldOmschrijving
            };
    }
    public async Task<List<OpdrachtDTO>> SearchAsync(string term, int limit = 50)
    {
        var query = _context.ProjectAssignments.Where(k => k.FldOpdrachtStr.Contains(term) || k.FldOmschrijving.Contains(term));
        return await query
            .Select(k => new OpdrachtDTO
            {
                Id = k.Id,
                OpdrachtNummer = k.FldOpdrachtStr,
                Omschrijving = k.FldOmschrijving,
                SoortOpdracht = k.FldSoort
            })
            .Take(limit)
            .ToListAsync();
    }
}