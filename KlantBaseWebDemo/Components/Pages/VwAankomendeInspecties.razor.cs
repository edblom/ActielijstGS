using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using Radzen.Blazor;
using KlantBaseWebDemo.Models.KlantBase;

namespace KlantBaseWebDemo.Components.Pages
{
    public partial class VwAankomendeInspecties
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        public KlantBaseService KlantBaseService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        protected IEnumerable<KlantBaseWebDemo.Models.KlantBase.VwAankomendeInspecty> vwAankomendeInspecties;
        protected RadzenDataGrid<KlantBaseWebDemo.Models.KlantBase.VwAankomendeInspecty> grid0;

        protected IEnumerable<Werknemer> werknemers;
        protected int selectedWerknemerId;

        // Bewaar alle inspecties om te filteren, initialiseer als lege lijst om null te vermijden
        private IEnumerable<VwAankomendeInspecty> allVwAankomendeInspecties = Enumerable.Empty<VwAankomendeInspecty>();

        protected override async Task OnInitializedAsync()
        {
            try
            {
                // Haal alle aankomende inspecties op en bewaar ze
                var queryableInspecties = await KlantBaseService.GetVwAankomendeInspecties();
                if (queryableInspecties != null)
                {
                    allVwAankomendeInspecties = await queryableInspecties.ToListAsync();
                }
                else
                {
                    Console.WriteLine("GetVwAankomendeInspecties retourneerde null");
                    allVwAankomendeInspecties = new List<VwAankomendeInspecty>(); // Fallback naar lege lijst
                }
                vwAankomendeInspecties = allVwAankomendeInspecties; // Start met alles tonen

                // Haal de werknemers op
                var queryableWerknemers = await KlantBaseService.GetWerknemers();
                if (queryableWerknemers != null)
                {
                    werknemers = await queryableWerknemers.ToListAsync();
                }
                else
                {
                    Console.WriteLine("GetWerknemers retourneerde null");
                    werknemers = new List<Werknemer>(); // Fallback naar lege lijst
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout in OnInitializedAsync: {ex.Message}");
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Fout", Detail = "Kan data niet laden." });
            }
        }

        protected void OnWerknemerSelected(object value)
        {
            if (value is int id && id > 0) // Controleer of een geldige ID is geselecteerd
            {
                selectedWerknemerId = id;

                // Haal de initialen van de geselecteerde werknemer op
                var selectedWerknemer = werknemers?.FirstOrDefault(w => w.WerknId == id);
                string initialen = selectedWerknemer?.Initialen;

                if (!string.IsNullOrEmpty(initialen) && allVwAankomendeInspecties != null)
                {
                    // Filter de inspecties waar InspecteurId of ExtraMedewerker overeenkomt met de initialen
                    vwAankomendeInspecties = allVwAankomendeInspecties.Where(i =>
                        (i.InspecteurId != null && i.InspecteurId == initialen) ||
                        (i.ExtraMedewerker != null && i.ExtraMedewerker == initialen));
                }
                else
                {
                    // Als geen geldige initialen of allVwAankomandeInspecties null is, toon alles
                    vwAankomendeInspecties = allVwAankomendeInspecties;
                }
            }
            else
            {
                // Als geen werknemer is geselecteerd (bijv. placeholder), toon alles
                vwAankomendeInspecties = allVwAankomendeInspecties;
                selectedWerknemerId = 0;
            }

            // Update de UI
            StateHasChanged();
        }
    }
}