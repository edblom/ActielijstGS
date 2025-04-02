using KlantBaseWebDemo.Models.KlantBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [Inject] protected DialogService DialogService { get; set; }

        protected RadzenDataGrid<TblMemo> grid0;
        protected IEnumerable<TblMemo> tblMemos;
        protected List<Werknemer> werknemers;
        protected HttpClient Http => HttpClientFactory.CreateClient("KlantBaseApi");

        protected override async Task OnInitializedAsync()
        {
            await LoadWerknemers();
            await LoadData();
        }

        protected async Task LoadWerknemers()
        {
            try
            {
                var unsortedWerknemers = await Http.GetFromJsonAsync<List<Werknemer>>("api/werknemers");
                if (unsortedWerknemers == null || !unsortedWerknemers.Any())
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Warning,
                        Summary = "Geen werknemers",
                        Detail = "Geen werknemers gevonden."
                    });
                    werknemers = new List<Werknemer>(); // Zorg ervoor dat werknemers niet null is
                }
                else
                {
                    // Sorteer de lijst op Initialen
                    werknemers = unsortedWerknemers.OrderBy(w => w.Initialen).ToList();
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Fout bij laden werknemers",
                    Detail = ex.Message
                });
                werknemers = new List<Werknemer>(); // Zorg ervoor dat werknemers niet null is bij een fout
            }
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
            await DialogService.OpenAsync<EditTblMemoApi>("Actie Bewerken",
                new Dictionary<string, object> { { "FldMid", item.FldMid } },
                new DialogOptions { Width = "700px", Height = "512px", Resizable = true, Draggable = true });

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

        protected string GetWerknemerInitialen(int? werknId)
        {
            if (werknId == null || werknemers == null) return "Onbekend";
            var werknemer = werknemers.FirstOrDefault(w => w.WerknId == werknId);
            return werknemer?.Initialen ?? "Onbekend";
        }
    }
}