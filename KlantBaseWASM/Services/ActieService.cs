using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KlantBaseWASM.Models;
using Microsoft.Extensions.Http;

namespace KlantBaseWASM.Services
{
    public class ActieService
    {
        private readonly HttpClient _httpClient;

        public ActieService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ActielijstAPI");
        }

        public async Task<List<Actie>> GetActionsAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Actie>>("api/acties") ?? new List<Actie>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error loading actions: " + ex.Message);
            }
        }

        public async Task<Actie> UpdateActionAsync(int id, Actie action)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/acties/{id}", action);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Actie>() ?? new Actie();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error updating action: " + ex.Message);
            }
        }
    }
}