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

        public async Task<ActieResponse> GetFilteredActionsAsync(string? searchTerm, string? status, int? werknemerId, int? actieSoortId, int? priorityId, int page = 1, int pageSize = 10, string? sortBy = null, string? sortDirection = "asc")
        {
            var query = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(searchTerm)) query["searchTerm"] = searchTerm;
            if (!string.IsNullOrWhiteSpace(status)) query["status"] = status;
            if (werknemerId.HasValue) query["werknemerId"] = werknemerId.ToString();
            if (actieSoortId.HasValue) query["actieSoortId"] = actieSoortId.ToString();
            if (priorityId.HasValue) query["priorityId"] = priorityId.ToString();
            query["page"] = page.ToString();
            query["pageSize"] = pageSize.ToString();
            if (!string.IsNullOrWhiteSpace(sortBy)) query["sortBy"] = sortBy;
            query["sortDirection"] = sortDirection;

            var queryString = string.Join("&", query.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            var response = await _httpClient.GetFromJsonAsync<ActieResponse>($"api/acties?{queryString}") ?? new ActieResponse { Items = new List<Actie>(), TotalCount = 0 };

            // Add initialen (client-side, as in original)
            foreach (var actie in response.Items)
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

            return response;
        }

        private string GenerateInitialen(string? voornaam, string? initialen)
        {
            if (string.IsNullOrEmpty(voornaam) || string.IsNullOrEmpty(initialen))
                return "-";
            return $"{initialen}".ToUpper();
        }
    }
}