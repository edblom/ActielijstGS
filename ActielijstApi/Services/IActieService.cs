using ActielijstApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActielijstApi.Services
{
    public interface IActieService
    {
        Task<List<Actie>> GetAllActiesAsync(); // Voor GET /api/acties
        Task<List<Actie>> GetActiesByUserAsync(int userId, string filterType); // Voor GET /api/acties/user/{userId}/{filterType}
        Task<Actie?> GetActieByIdAsync(int id); // Voor GET /api/acties/{id}
        Task<Actie> CreateActieAsync(Actie actie); // Voor POST /api/acties
        Task<bool> UpdateActieAsync(int id, Actie actie); // Voor PUT /api/acties/{id}
        Task<bool> PatchActieAsync(int id, Dictionary<string, object> updates); // Voor PATCH /api/acties/{id}
        Task<bool> DeleteActieAsync(int id); // Voor DELETE /api/acties/{id}
    }
}