using ActielijstApi.Data;
using ActielijstApi.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ProjectService
{
    private readonly ApplicationDbContext _context;

    public ProjectService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectDTO> GetByIdAsync(int id)
    {
        var project = await _context.Projecten
            .Where(p => p.Id == id)
            .FirstOrDefaultAsync();
        return project == null
            ? new ProjectDTO
            {
                Id = 0,
                ProjectNummer = "Onbekend",
                Omschrijving = "Onbekend"
            }
            : new ProjectDTO
            {
                Id = project.Id,
                ProjectNummer = project.FldExternNummer,
                Omschrijving = project.FldProjectNaam
            };
    }
    public async Task<List<ProjectDTO>> SearchAsync(string term, int limit = 50)
    {
        var query = _context.Projecten.Where(k => k.FldExternNummer.Contains(term) || k.FldProjectNaam.Contains(term));
        return await query
            .Select(k => new ProjectDTO
            {
                Id = k.Id,
                ProjectNummer = k.FldExternNummer,
                Omschrijving = k.FldProjectNaam,
            })
            .Take(limit)
            .ToListAsync();
    }
}