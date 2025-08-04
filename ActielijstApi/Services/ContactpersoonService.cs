using ActielijstApi.Data;
using ActielijstApi.Dtos;
using Microsoft.EntityFrameworkCore;
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
}
