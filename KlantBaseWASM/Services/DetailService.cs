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

        public async Task<string> GetKlantDetailsAsync(int? id)
        {
            if (id.HasValue && id.Value != 0)
            {
                try
                {
                    var response = await _httpClient.GetFromJsonAsync<KlantDTO>($"api/klant/{id.Value}");
                    return response != null
                        ? $"Naam: {response.Naam}, Plaats: {response.Plaats}, Zoekcode: {response.Zoekcode}"
                        : "Onbekend (geen response)";
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"HTTP Error fetching klant: {ex.Message}");
                    return "Onbekend (netwerkfout)";
                }
            }
            Console.WriteLine($"KlantId is null or 0: {id}");
            return "Onbekend (ongeldig ID)";
        }

        public async Task<string> GetProjectDetailsAsync(int? id)
        {
            if (id.HasValue && id.Value != 0)
            {
                try
                {
                    var response = await _httpClient.GetFromJsonAsync<ProjectDTO>($"api/projecten/{id.Value}");
                    return response != null
                        ? $"Nummer: {response.ProjectNummer}, Omschrijving: {response.Omschrijving}"
                        : "Onbekend (geen response)";
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"HTTP Error fetching project: {ex.Message}");
                    return "Onbekend (netwerkfout)";
                }
            }
            Console.WriteLine($"ProjectId is null or 0: {id}");
            return "Onbekend (ongeldig ID)";
        }

        public async Task<string> GetContactpersoonDetailsAsync(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                if (int.TryParse(id, out int contactId))
                {
                    try
                    {
                        var response = await _httpClient.GetFromJsonAsync<ContactpersoonDTO>($"api/contactpersonen/{contactId}");
                        return response != null
                            ? $"Naam: {response.VolledigeNaam}"
                            : "Onbekend (geen response)";
                    }
                    catch (HttpRequestException ex)
                    {
                        Console.WriteLine($"HTTP Error fetching contactpersoon by ID: {ex.Message}");
                        return "Onbekend (netwerkfout)";
                    }
                }
                // Als het geen getal is, toon de string direct
                Console.WriteLine($"ContactPersId is a string: {id}");
                return id; // Direct de string tonen
            }
            Console.WriteLine("ContactPersId is null or empty");
            return "Onbekend (ongeldig ID)";
        }
        public async Task<string> GetOpdrachtDetailsAsync(int? id)
        {
            if (id.HasValue && id.Value != 0)
            {
                try
                {
                    var response = await _httpClient.GetFromJsonAsync<OpdrachtDTO>($"api/opdrachten/{id.Value}");
                    return response != null
                        ? $"Nummer: {response.OpdrachtNummer}, Soort: {response.SoortOpdracht}, Omschrijving: {response.Omschrijving}"
                        : "Onbekend (geen response)";
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"HTTP Error fetching opdracht: {ex.Message}");
                    return "Onbekend (netwerkfout)";
                }
            }
            Console.WriteLine($"OpdrachtId is null or 0: {id}");
            return "Onbekend (ongeldig ID)";
        }
    }
}