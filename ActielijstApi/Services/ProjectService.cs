using ActielijstApi.Data;
using ActielijstApi.Dtos;
using Microsoft.EntityFrameworkCore;
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
}