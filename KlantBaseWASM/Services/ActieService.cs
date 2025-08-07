using KlantBaseWASM.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace KlantBaseWASM.Services
{
    public class ActieService
    {
        private readonly HttpClient _httpClient;
        private readonly WerknemerService _werknemerService;
        private List<Werknemer> _werknemers;

        public ActieService(IHttpClientFactory httpClientFactory, WerknemerService werknemerService)
        {
            _httpClient = httpClientFactory.CreateClient("ActielijstAPI");
            _werknemerService = werknemerService;
            Console.WriteLine("ActieService instantiated without caching.");
        }

        public async Task InitializeAsync()
        {
            _werknemers = await _werknemerService.GetWerknemersAsync();
            Console.WriteLine($"Werknemers initialized with {_werknemers.Count} items.");
        }

        public async Task<ActieResponse> GetFilteredActionsAsync(
            string? searchTerm =null,
            string? status =null,
            int? werknemerId = null,
            int? actieSoortId = null,
            int? priorityId = null,
            int page = 1,
            int pageSize = 0,
            string? sortBy = null,
            string? sortDirection = "asc")
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
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
            Console.WriteLine($"WASM querystring ActieService response: {stopwatch.ElapsedMilliseconds} ms");
            var queryString = string.Join("&", query.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            var response = await _httpClient.GetFromJsonAsync<ActieResponse>($"api/acties?{queryString}") ?? new ActieResponse { Items = new List<Actie>(), TotalCount = 0 };
            Console.WriteLine($"WASM before initialen ActieService response: {stopwatch.ElapsedMilliseconds} ms");
            // Add initialen (client-side)
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
            stopwatch.Stop();
            Console.WriteLine($"WASM ActieServie response: {stopwatch.ElapsedMilliseconds} ms");
            return response;
        }

        public async Task<ActieResponse> GetFilteredActionsByWerknIdAsync(
            string? searchTerm,
            string? status,
            int? actieSoortId,
            int? priorityId,
            int? werknId,
            int page = 1,
            int pageSize = 10,
            string? sortBy = null,
            string? sortDirection = "asc")
        {
            var query = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(searchTerm)) query["searchTerm"] = searchTerm;
            if (!string.IsNullOrWhiteSpace(status)) query["status"] = status;
            if (actieSoortId.HasValue) query["actieSoortId"] = actieSoortId.ToString();
            if (priorityId.HasValue) query["priorityId"] = priorityId.ToString();
            if (werknId.HasValue) query["werknId"] = werknId.ToString();
            query["page"] = page.ToString();
            query["pageSize"] = pageSize.ToString();
            if (!string.IsNullOrWhiteSpace(sortBy)) query["sortBy"] = sortBy;
            query["sortDirection"] = sortDirection;

            var queryString = string.Join("&", query.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
            var response = await _httpClient.GetFromJsonAsync<ActieResponse>($"api/acties?{queryString}") ?? new ActieResponse { Items = new List<Actie>(), TotalCount = 0 };

            // Add initialen (client-side)
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