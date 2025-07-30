using KlantBaseWASM.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace KlantBaseWASM.Services
{
    public class ActieSoortService
    {
        private readonly HttpClient _httpClient;

        public ActieSoortService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ActielijstAPI"); // Gebruik named client
            Console.WriteLine($"ActieSoortService BaseAddress: {_httpClient.BaseAddress}");
        }

        public async Task<List<ActieSoort>> GetAllActiesoortenAsync()
        {
            try
            {
                var actiesoorten = await _httpClient.GetFromJsonAsync<List<ActieSoort>>("api/actiesoorten/all") ?? new List<ActieSoort>();
                Console.WriteLine($"Retrieved {actiesoorten.Count} actiesoorten.");
                return actiesoorten;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching actiesoorten: {ex.Message}");
                return new List<ActieSoort>();
            }
        }
    }
}