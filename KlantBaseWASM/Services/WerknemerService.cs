using KlantBaseWASM.Models; // Voor het Werknemer-model
using Microsoft.JSInterop; // Voor IJSRuntime en Console.WriteLine in browser
using System.Collections.Generic; // Voor List<>
using System.Net.Http; // Voor HttpClient en IHttpClientFactory
using System.Net.Http.Json;
using System.Threading.Tasks; // Voor async/await en Task

namespace KlantBaseWASM.Services
{
    public class WerknemerService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime; // Optioneel voor logging

        public WerknemerService(IHttpClientFactory httpClientFactory, IJSRuntime jsRuntime = null)
        {
            _httpClient = httpClientFactory.CreateClient("ActielijstAPI"); // Gebruik named client
            _jsRuntime = jsRuntime;
            Console.WriteLine("WerknemerService instantiated without caching.");
        }

        public async Task<List<Werknemer>> GetWerknemersAsync()
        {
            try
            {
                //Console.WriteLine("Starting GetWerknemersAsync call...");
                //Console.WriteLine($"Current BaseAddress: {_httpClient.BaseAddress}");

                var response = await _httpClient.GetAsync("api/Werknemers");
                Console.WriteLine($"API Status Code: {response.StatusCode}");

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"API Error: {response.ReasonPhrase}");
                    return new List<Werknemer>();
                }

                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Response Content: {content}");
                var werknemers = await response.Content.ReadFromJsonAsync<List<Werknemer>>() ?? new List<Werknemer>();

                Console.WriteLine($"Retrieved {werknemers.Count} werknemers.");
                return werknemers;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new List<Werknemer>();
            }
        }
    }
}