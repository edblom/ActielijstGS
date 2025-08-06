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

        public async Task<ActieResponse> GetFilteredActiesAsync(
            string? searchTerm,
            string? status,
            int? werknemerId,
            string? actieSoortId,
            int? priorityId,
            int? werknId,
            int page,
            int pageSize,
            string? sortBy,
            string? sortDirection)
        {
            var query = _context.Acties.AsQueryable();

            // Filtering
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(a => a.FldOmschrijving != null && a.FldOmschrijving.ToLower().Contains(searchTerm.ToLower()));
            }
            if (!string.IsNullOrWhiteSpace(status))
            {
                if (status == "Gereed")
                {
                    query = query.Where(a => a.FldMActieGereed != null);
                }
                else if (status == "Openstaand")
                {
                    query = query.Where(a => a.FldMActieGereed == null);
                }
            }
            if (werknemerId.HasValue)
            {
                query = query.Where(a => a.FldMActieVoor == werknemerId || a.FldMActieVoor2 == werknemerId);
            }
            if (!string.IsNullOrWhiteSpace(actieSoortId))
            {
                query = query.Where(a => a.FldMActieSoort == actieSoortId);
            }
            if (priorityId.HasValue)
            {
                query = query.Where(a => a.FldMPrioriteit == priorityId);
            }
            if (werknId.HasValue)
            {
                query = query.Where(a => a.WerknId == werknId);
            }

            // Total count for paging
            var totalCount = await query.CountAsync();

            // Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortDirection?.ToLower() == "desc")
                {
                    query = sortBy.ToLower() switch
                    {
                        "fldmactiedatum" => query.OrderByDescending(a => a.FldMActieDatum),
                        "fldomschrijving" => query.OrderByDescending(a => a.FldOmschrijving),
                        "fldmprioriteit" => query.OrderByDescending(a => a.FldMPrioriteit),
                        _ => query.OrderBy(a => a.FldMActieDatum)
                    };
                }
                else
                {
                    query = sortBy.ToLower() switch
                    {
                        "fldmactiedatum" => query.OrderBy(a => a.FldMActieDatum),
                        "fldomschrijving" => query.OrderBy(a => a.FldOmschrijving),
                        "fldmprioriteit" => query.OrderBy(a => a.FldMPrioriteit),
                        _ => query.OrderBy(a => a.FldMActieDatum)
                    };
                }
            }
            else
            {
                query = query.OrderBy(a => a.FldMActieDatum);
            }

            // Paging (pageSize = 0 means no paging)
            if (pageSize > 0)
            {
                query = query.Skip((page - 1) * pageSize).Take(pageSize);
            }

            // Execute query
            var acties = await query.ToListAsync();

            return new ActieResponse
            {
                Items = acties,
                TotalCount = totalCount
            };
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
                actie.FldMActieGereed = updates.fldMActieGereed.Value;
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