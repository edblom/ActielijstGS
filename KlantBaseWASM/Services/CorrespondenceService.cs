using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KlantBaseWASM.Models;
using Microsoft.Extensions.Http;

namespace KlantBaseWASM.Services
{
    public class CorrespondenceService
    {
        private readonly HttpClient _httpClient;

        public CorrespondenceService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ActielijstAPI");
        }

        public async Task<List<CorrespondentieDto>> GetCorrespondentieAsync(int? klantId = null, int? projectId = null, int? opdrachtId = null, int pageNumber = 1, int pageSize = 50)
        {
            try
            {
                var query = $"?pageNumber={pageNumber}&pageSize={pageSize}";
                if (klantId.HasValue) query += $"&klantId={klantId}";
                if (projectId.HasValue) query += $"&projectId={projectId}";
                if (opdrachtId.HasValue) query += $"&opdrachtId={opdrachtId}";

                return await _httpClient.GetFromJsonAsync<List<CorrespondentieDto>>($"api/Correspondence{query}") ?? new List<CorrespondentieDto>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error loading correspondence: " + ex.Message);
            }
        }

        public async Task<List<Adres>> GetKlantenAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Adres>>("api/adres") ?? new List<Adres>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error loading customers: " + ex.Message);
            }
        }

        public async Task<List<TblProjecten>> GetProjectenAsync(int? klantId = null)
        {
            try
            {
                var query = klantId.HasValue ? $"?klantId={klantId}" : "";
                return await _httpClient.GetFromJsonAsync<List<TblProjecten>>($"api/projecten{query}") ?? new List<TblProjecten>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error loading projects: " + ex.Message);
            }
        }

        public async Task<List<TblOpdrachten>> GetOpdrachtenAsync(int? projectId = null)
        {
            try
            {
                var query = projectId.HasValue ? $"?projectId={projectId}" : "";
                return await _httpClient.GetFromJsonAsync<List<TblOpdrachten>>($"api/opdrachten{query}") ?? new List<TblOpdrachten>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error loading assignments: " + ex.Message);
            }
        }

        public async Task<List<Contactpersonen>> GetContactpersonenAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Contactpersonen>>("api/contactpersonen") ?? new List<Contactpersonen>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error loading contact persons: " + ex.Message);
            }
        }

        public async Task<List<StandaardDoc>> GetSjablonenAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<StandaardDoc>>("api/stSjablonen") ?? new List<StandaardDoc>();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error loading templates: " + ex.Message);
            }
        }

        public async Task<GenerateCorrespondenceResponse> GenerateDocumentAsync(GenerateCorrespondenceRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Correspondence/generate", request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<GenerateCorrespondenceResponse>() ?? new GenerateCorrespondenceResponse();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error generating document: " + ex.Message);
            }
        }

        public async Task<SendEmailResponse> SendEmailAsync(SendEmailRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Correspondence/send-email", request);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<SendEmailResponse>() ?? new SendEmailResponse();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error sending email: " + ex.Message);
            }
        }

        public async Task<byte[]> DownloadDocumentAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Correspondence/open/by-correspondence/{id}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsByteArrayAsync();
            }
            catch (HttpRequestException ex)
            {
                throw new Exception("Error downloading document: " + ex.Message);
            }
        }
    }
}