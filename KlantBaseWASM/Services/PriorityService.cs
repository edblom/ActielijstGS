using KlantBaseWASM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace KlantBaseWASM.Services
{
    public class PriorityService
    {
        private readonly HttpClient _httpClient;

        public PriorityService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ActielijstAPI");
        }

        public async Task<List<Priority>> GetAllPrioritiesAsync()
        {
            try
            {
                var priorities = await _httpClient.GetFromJsonAsync<List<Priority>>("api/priorities") ?? new List<Priority>();
                Console.WriteLine($"Retrieved {priorities.Count} priorities.");
                return priorities;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching priorities: {ex.Message}");
                return new List<Priority>();
            }
        }
    }
}