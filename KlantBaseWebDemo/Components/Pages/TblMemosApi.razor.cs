using KlantBaseWebDemo.Models.KlantBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace KlantBaseWebDemo.Components.Pages
{
    [Authorize]
    public class TblMemosApiBase : ComponentBase
    {
        [Inject] protected IHttpClientFactory HttpClientFactory { get; set; }
        [Inject] protected NavigationManager Navigation { get; set; }
        [Inject] protected NotificationService NotificationService { get; set; }
        [Inject] protected DialogService DialogService { get; set; } // Toegevoegd voor dialoog

        protected RadzenDataGrid<TblMemo> grid0;
        protected IEnumerable<TblMemo> tblMemos;
        protected HttpClient Http => HttpClientFactory.CreateClient("KlantBaseApi");

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        protected async Task LoadData()
        {
            try
            {
                tblMemos = await Http.GetFromJsonAsync<List<TblMemo>>("api/acties");
                if (tblMemos == null || !tblMemos.Any())
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Warning,
                        Summary = "Geen data",
                        Detail = "Geen acties gevonden."
                    });
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Fout bij laden",
                    Detail = ex.Message
                });
            }
        }

        protected async Task EditRow(TblMemo item)
        {
            // Open een dialoogvenster met EditTblMemoApi
            await DialogService.OpenAsync<EditTblMemoApi>("Actie Bewerken",
                new Dictionary<string, object> { { "FldMid", item.FldMid } },
                new DialogOptions { Width = "700px", Height = "512px", Resizable = true, Draggable = true });

            // Herlaad de lijst na het sluiten van de dialoog
            await LoadData();
            await grid0.Reload();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            var newMemo = new TblMemo
            {
                FldOmschrijving = "Nieuwe actie",
                FldMactieDatum = DateTime.Now,
                FldMprioriteit = 1,
                FldMcontactPers = "",
                FldMafspraak = "",
                FldMactieSoort = ""
            };

            var response = await Http.PostAsJsonAsync("api/acties", newMemo);
            if (response.IsSuccessStatusCode)
            {
                await LoadData();
                await grid0.Reload();
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Toegevoegd",
                    Detail = "Actie succesvol toegevoegd."
                });
            }
            else
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Fout bij toevoegen",
                    Detail = $"Status: {response.StatusCode}"
                });
            }
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, TblMemo tblMemo)
        {
            var response = await Http.DeleteAsync($"api/acties/{tblMemo.FldMid}");
            if (response.IsSuccessStatusCode)
            {
                await LoadData();
                await grid0.Reload();
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Verwijderd",
                    Detail = "Actie succesvol verwijderd."
                });
            }
            else
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Fout bij verwijderen",
                    Detail = $"Status: {response.StatusCode}"
                });
            }
        }
    }
}