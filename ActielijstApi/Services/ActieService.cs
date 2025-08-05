using ActielijstApi.Data;
using KlantBaseShare.Dtos;
using ActielijstApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            // Update alle velden behalve SSMA_TimeStamp
            existingActie.FldMDatum = actie.FldMDatum;
            existingActie.WerknId = actie.WerknId;
            existingActie.FldMKlantId = actie.FldMKlantId;
            existingActie.FldMContactPers = actie.FldMContactPers;
            existingActie.FldMOfferteId = actie.FldMOfferteId;
            existingActie.FldMProjectId = actie.FldMProjectId;
            existingActie.FldOpdrachtId = actie.FldOpdrachtId;
            existingActie.FldOmschrijving = actie.FldOmschrijving;
            existingActie.FldMAfspraak = actie.FldMAfspraak;
            existingActie.FldMActieDatum = actie.FldMActieDatum;
            existingActie.FldMActieVoor = actie.FldMActieVoor;
            existingActie.FldMActieVoor2 = actie.FldMActieVoor2;
            existingActie.FldMActieGereed = actie.FldMActieGereed;
            existingActie.FldMActieSoort = actie.FldMActieSoort;
            existingActie.FldMPrioriteit = actie.FldMPrioriteit;
            // SSMA_TimeStamp wordt automatisch bijgewerkt door de database

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine($"Concurrency conflict: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating action: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> PatchActieAsync(int id, PatchActieDto updates)
        {
            var actie = await _context.Acties.FirstOrDefaultAsync(a => a.FldMid == id);
            if (actie == null) return false;

            if (updates.fldMActieGereed.HasValue)
            {
                actie.FldMActieGereed = updates.fldMActieGereed.Value; // Kleine 'f' gebruikt
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