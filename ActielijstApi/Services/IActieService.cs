using ActielijstApi.Dtos;
using ActielijstApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActielijstApi.Services
{
    public interface IActieService
    {
        Task<List<Actie>> GetAllActiesAsync();
        Task<List<Actie>> GetActiesByUserAsync(int userId, string filterType);
        Task<Actie?> GetActieByIdAsync(int id);
        Task<Actie> CreateActieAsync(Actie actie);
        Task<bool> UpdateActieAsync(int id, Actie actie);
        Task<bool> PatchActieAsync(int id, PatchActieDto updates); // Gebruik DTO
        Task<bool> DeleteActieAsync(int id);
    }
}