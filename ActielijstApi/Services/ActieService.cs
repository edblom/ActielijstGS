using ActielijstApi.Data;
using ActielijstApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace ActielijstApi.Services
{
    public class ActieService : IActieService
    {
        private readonly ApplicationDbContext _context;

        public ActieService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Actie>> GetAllActiesAsync()
        {
            return await _context.Acties.ToListAsync();
        }

        public async Task<List<Actie>> GetActiesByUserAsync(int userId, string filterType)
        {
            var acties = filterType.ToLower() switch
            {
                "assigned" => await _context.Acties.Where(a => (a.FldMActieVoor.HasValue && a.FldMActieVoor.Value == userId) || (a.FldMActieVoor2.HasValue && a.FldMActieVoor2.Value == userId)).ToListAsync(),
                "created" => await _context.Acties.Where(a => a.WerknId.HasValue && a.WerknId.Value == userId).ToListAsync(),
                _ => await _context.Acties.Where(a => (a.FldMActieVoor.HasValue && a.FldMActieVoor.Value == userId) || (a.FldMActieVoor2.HasValue && a.FldMActieVoor2.Value == userId) || (a.WerknId.HasValue && a.WerknId.Value == userId)).ToListAsync()
            };
            return acties;
        }

        public async Task<Actie?> GetActieByIdAsync(int id)
        {
            return await _context.Acties.FirstOrDefaultAsync(a => a.FldMid == id);
        }

        public async Task<Actie> CreateActieAsync(Actie actie)
        {
            _context.Acties.Add(actie);
            await _context.SaveChangesAsync();
            return actie;
        }

        public async Task<bool> UpdateActieAsync(int id, Actie actie)
        {
            var existingActie = await _context.Acties.FirstOrDefaultAsync(a => a.FldMid == id);
            if (existingActie == null) return false;

            existingActie.FldOmschrijving = actie.FldOmschrijving;
            existingActie.FldMActieVoor = actie.FldMActieVoor;
            existingActie.FldMActieVoor2 = actie.FldMActieVoor2;
            existingActie.FldMActieDatum = actie.FldMActieDatum;
            existingActie.FldMActieSoort = actie.FldMActieSoort;
            existingActie.WerknId = actie.WerknId;
            existingActie.FldMPrioriteit = actie.FldMPrioriteit;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PatchActieAsync(int id, Dictionary<string, object> updates)
        {
            var actie = await _context.Acties.FirstOrDefaultAsync(a => a.FldMid == id);
            if (actie == null) return false;

            foreach (var update in updates)
            {
                switch (update.Key.ToLower())
                {
                    case "fldmdatum": actie.FldMDatum = update.Value != null ? Convert.ToDateTime(update.Value) : null; break;
                    case "werknid": actie.WerknId = update.Value != null ? Convert.ToInt32(update.Value) : null; break;
                    case "fldmklantid": actie.FldMKlantId = update.Value != null ? Convert.ToInt32(update.Value) : null; break;
                    case "fldmcontactpers": actie.FldMContactPers = update.Value?.ToString(); break;
                    case "fldmofferteid": actie.FldMOfferteId = update.Value != null ? Convert.ToInt32(update.Value) : null; break;
                    case "fldmprojectid": actie.FldMProjectId = update.Value != null ? Convert.ToInt32(update.Value) : null; break;
                    case "fldopdrachtid": actie.FldOpdrachtId = update.Value != null ? Convert.ToInt32(update.Value) : null; break;
                    case "fldomschrijving": actie.FldOmschrijving = update.Value?.ToString(); break;
                    case "fldmafspraak": actie.FldMAfspraak = update.Value?.ToString(); break;
                    case "fldmactiedatum": actie.FldMActieDatum = update.Value != null ? Convert.ToDateTime(update.Value) : null; break;
                    case "fldmactievoor": actie.FldMActieVoor = update.Value != null ? Convert.ToInt32(update.Value) : null; break;
                    case "fldmactievoor2": actie.FldMActieVoor2 = update.Value != null ? Convert.ToInt32(update.Value) : null; break;
                    case "fldmactiegereed": actie.FldMActieGereed = update.Value != null ? Convert.ToDateTime(update.Value) : null; break;
                    case "fldmactiesoort": actie.FldMActieSoort = update.Value?.ToString(); break;
                    case "fldmprioriteit": actie.FldMPrioriteit = update.Value != null ? Convert.ToInt32(update.Value) : null; break;
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteActieAsync(int id)
        {
            var actie = await _context.Acties.FirstOrDefaultAsync(a => a.FldMid == id);
            if (actie == null) return false;

            _context.Acties.Remove(actie);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}