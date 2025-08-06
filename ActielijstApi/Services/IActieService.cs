using KlantBaseShare.Dtos;
using ActielijstApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActielijstApi.Services
{
    public interface IActieService
    {
        Task<List<Actie>> GetAllActiesAsync();
        Task<ActieResponse> GetFilteredActiesAsync(
            string? searchTerm,
            string? status,
            int? werknemerId,
            string? actieSoortId,
            int? priorityId,
            int? werknId,
            int page,
            int pageSize,
            string? sortBy,
            string? sortDirection);
        Task<List<Actie>> GetActiesByUserAsync(int userId, string filterType);
        Task<Actie?> GetActieByIdAsync(int id);
        Task<Actie> CreateActieAsync(Actie actie);
        Task<bool> UpdateActieAsync(int id, Actie actie);
        Task<bool> PatchActieAsync(int id, PatchActieDto updates);
        Task<bool> DeleteActieAsync(int id);
    }
}