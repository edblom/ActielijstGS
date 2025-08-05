using KlantBaseWASM.Dtos;
using Microsoft.AspNetCore.Components;
using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace KlantBaseWASM.Services
{
    public class DetailsService
    {
        private readonly HttpClient _httpClient;

        public DetailsService(IHttpClientFactory clientFactory)
        {
            _httpClient = clientFactory.CreateClient("ActielijstAPI");
        }

        public async Task<KlantDTO> GetKlantDetailsAsync(int? id)
        {
            if (id.HasValue && id.Value != 0)
            {
                try
                {
                    var response = await _httpClient.GetFromJsonAsync<KlantDTO>($"api/klant/{id.Value}");
                    return response;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"HTTP Error fetching klant: {ex.Message}");
                    return null;
                }
            }
            Console.WriteLine($"KlantId is null or 0: {id}");
            return null;
        }

        public async Task<ProjectDTO> GetProjectDetailsAsync(int? id)
        {
            if (id.HasValue && id.Value != 0)
            {
                try
                {
                    var response = await _httpClient.GetFromJsonAsync<ProjectDTO>($"api/projecten/{id.Value}");
                    return response;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"HTTP Error fetching project: {ex.Message}");
                    return null;
                }
            }
            Console.WriteLine($"ProjectId is null or 0: {id}");
            return null;
        }

        public async Task<ContactpersoonDTO> GetContactpersoonDetailsAsync(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (int.TryParse(id, out int contactId))
                {
                    try
                    {
                        var response = await _httpClient.GetFromJsonAsync<ContactpersoonDTO>($"api/contactpersonen/{contactId}");
                        return response;
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"HTTP Error fetching contactpersoon by ID: {ex.Message}");
                        return null;
                    }
                }
                // Als het geen getal is, toon de string direct
                Console.WriteLine($"ContactPersId is a string: {id}");
                return null; // Direct de string tonen
            }
            Console.WriteLine("ContactPersId is null or empty");
            return null;
        }
        public async Task<OpdrachtDTO> GetOpdrachtDetailsAsync(int? id)
        {
            if (id.HasValue && id.Value != 0)
            {
                try
                {
                    var response = await _httpClient.GetFromJsonAsync<OpdrachtDTO>($"api/opdrachten/{id.Value}");
                    return response;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"HTTP Error fetching opdracht: {ex.Message}");
                    return null;
                }
            }
            Console.WriteLine($"OpdrachtId is null or 0: {id}");
            return null;
        }
        public async Task<List<KlantDTO>> SearchKlantAsync(string term)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<KlantDTO>>($"api/klant/search?term={term}&limit=50");
                return response ?? new List<KlantDTO>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error searching klant: {ex.Message}");
                return new List<KlantDTO>();
            }
        }
        public async Task<List<ProjectDTO>> SearchProjectAsync(string term)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<ProjectDTO>>($"api/projecten/search?term={term}&limit=50");
                return response ?? new List<ProjectDTO>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error searching project: {ex.Message}");
                return new List<ProjectDTO>();
            }
        }
        public async Task<List<ContactpersoonDTO>> SearchContactpersoonAsync(string term)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<ContactpersoonDTO>>($"api/Contactpersonen/search?term={term}&limit=50");
                return response ?? new List<ContactpersoonDTO>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error searching contactpersoon: {ex.Message}");
                return new List<ContactpersoonDTO>();
            }
        }

        public async Task<List<OpdrachtDTO>> SearchOpdrachtAsync(string term)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<OpdrachtDTO>>($"api/Opdrachten/search?term={term}&limit=50");
                return response ?? new List<OpdrachtDTO>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error searching opdracht: {ex.Message}");
                return new List<OpdrachtDTO>();
            }
        }
    }
}