using KlantBaseWASM.Models; // Voor het Actie- en Werknemer-model
using Microsoft.JSInterop; // Voor IJSRuntime en Console.WriteLine in browser
using System.Collections.Generic; // Voor List<>
using System.Net.Http; // Voor HttpClient en IHttpClientFactory
using System.Net.Http.Json;
using System.Threading.Tasks; // Voor async/await en Task

namespace KlantBaseWASM.Services
{
    public class ActieService
    {
        private readonly HttpClient _httpClient;
        private readonly WerknemerService _werknemerService;
        private List<Werknemer> _werknemers; // Tijdelijke lijst, geen cache

        public ActieService(IHttpClientFactory httpClientFactory, WerknemerService werknemerService)
        {
            _httpClient = httpClientFactory.CreateClient("ActielijstAPI"); // Gebruik named client
            _werknemerService = werknemerService;
            Console.WriteLine("ActieService instantiated without caching.");
        }

        public async Task InitializeAsync()
        {
            _werknemers = await _werknemerService.GetWerknemersAsync(); // Haal werknemers direct op
            Console.WriteLine($"Werknemers initialized with {_werknemers.Count} items.");
        }

        public async Task<List<Actie>> GetActionsAsync()
        {
            if (_werknemers == null || _werknemers.Count == 0)
            {
                await InitializeAsync();
            }

            var actions = await _httpClient.GetFromJsonAsync<List<Actie>>("api/acties") ?? new List<Actie>();

            foreach (var actie in actions)
            {
                var werknemerVoor = _werknemers.FirstOrDefault(w => w.WerknId == actie.FldMActieVoor);
                actie.FldMActieVoorInitialen = werknemerVoor != null
                    ? GenerateInitialen(werknemerVoor.Voornaam, werknemerVoor.Initialen)
                    : "Onbekend";
                var werknemerVoor2 = _werknemers.FirstOrDefault(w => w.WerknId == actie.FldMActieVoor2);
                actie.FldMActieVoor2Initialen = werknemerVoor2 != null
                    ? GenerateInitialen(werknemerVoor2.Voornaam, werknemerVoor2.Initialen)
                    : "Onbekend";
            }

            return actions;
        }

        private string GenerateInitialen(string? voornaam, string? initialen)
        {
            if (string.IsNullOrEmpty(voornaam) || string.IsNullOrEmpty(initialen))
                return "Onbekend";
            return $"{voornaam[0]}{initialen}".ToUpper();
        }
    }
}