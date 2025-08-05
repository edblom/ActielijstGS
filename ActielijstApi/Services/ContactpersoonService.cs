using ActielijstApi.Data;
using KlantBaseShare.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class ContactpersoonService
{
    private readonly ApplicationDbContext _context;

    public ContactpersoonService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ContactpersoonDTO> GetByIdAsync(int id)
    {
        var contact = await _context.ContactPersonen
            .Where(c => c.Id == id)
            .FirstOrDefaultAsync(); // Moet nu werken met juiste using
        return contact == null
            ? new ContactpersoonDTO
            {
                Id = 0, // Of een andere standaardwaarde
                Voornaam = "Onbekend",
                Initialen = "",
                Tussenvoegsel = "",
                Achternaam = ""
            }
            : new ContactpersoonDTO
            {
                Id = contact.Id, // Gebruik contact.Id in plaats van FldContactId
                Voornaam = contact.roepnaam, // FldVoornaam lijkt niet te bestaan, gebruik roepnaam
                Initialen = contact.voorletters,
                Tussenvoegsel = contact.tussenvoegsel,
                Achternaam = contact.achternaam
            };
    }

    public async Task<List<ContactpersoonDTO>> SearchAsync(string term, int limit = 50)
    {
        var query = _context.ContactPersonen.Where(k => k.achternaam.Contains(term) || k.roepnaam.Contains(term));
        return await query
            .Select(k => new ContactpersoonDTO
            {
                Voornaam = k.roepnaam, // FldVoornaam lijkt niet te bestaan, gebruik roepnaam
                Initialen = k.voorletters,
                Tussenvoegsel = k.tussenvoegsel,
                Achternaam = k.achternaam
            })
            .Take(limit)
            .ToListAsync();
    }
}

