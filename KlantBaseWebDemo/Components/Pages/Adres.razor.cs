using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using KlantBaseWebDemo.Models.KlantBase;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;

namespace KlantBaseWebDemo.Components.Pages
{
    public partial class Adres
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

        protected IEnumerable<KlantBaseWebDemo.Models.KlantBase.Adre> adres;
        protected IEnumerable<KlantBaseWebDemo.Models.KlantBase.Adre> allAdres;
        protected RadzenDataGrid<KlantBaseWebDemo.Models.KlantBase.Adre> grid0;
        protected string searchText;
        protected bool isLoading;

        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            try
            {
                var queryableAdres = await KlantBaseService.GetAdres();
                if (queryableAdres != null)
                {
                    allAdres = await queryableAdres.ToListAsync();
                    adres = allAdres;
                    Console.WriteLine($"Aantal adressen geladen: {allAdres.Count()}");
                }
                else
                {
                    Console.WriteLine("GetAdres retourneerde null");
                    allAdres = new List<KlantBaseWebDemo.Models.KlantBase.Adre>();
                    adres = allAdres;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fout in OnInitializedAsync: {ex.Message}");
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Fout",
                    Detail = "Kan adressen niet laden."
                });
            }
            finally
            {
                isLoading = false;
            }
        }

        protected async Task SearchTextChanged(string value)
        {
            searchText = value;
            Console.WriteLine($"SearchTextChanged aangeroepen met waarde: '{searchText}'");

            await Task.Delay(300); // 300ms debounce
            FilterAdres();
            await grid0.Reload();
        }

        private void FilterAdres()
        {
            Console.WriteLine($"FilterAdres aangeroepen met zoekterm: '{searchText}'");
            if (!string.IsNullOrEmpty(searchText) && searchText.Length >= 2 && allAdres != null)
            {
                adres = allAdres.Where(a => ContainsSearchText(a, searchText));
                Console.WriteLine($"Gefilterd resultaat: {adres.Count()} adressen");
            }
            else
            {
                adres = allAdres;
                Console.WriteLine("Geen filter toegepast, toon alle adressen");
            }
        }

        private bool ContainsSearchText(KlantBaseWebDemo.Models.KlantBase.Adre adre, string search)
        {
            if (string.IsNullOrEmpty(search)) return true;

            return (adre.Zoekcode?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (adre.Bedrijf?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (adre.Tav?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (adre.Geachte?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (adre.VestigAdr?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (adre.VestigPc?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (adre.VestigPlaats?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (adre.Postadres?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (adre.Pc?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (adre.Wpl?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (adre.Land?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (adre.Tel?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (adre.TelPrive?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (adre.Fax?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (adre.MobelTel?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (adre.Categorie?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (adre.Omschr?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (adre.EMailAdr?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (adre.Opmerkingen?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (adre.Voorletters?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (adre.Roepnaam?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (adre.Voorvoegsel?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (adre.Tussenvoegsel?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
                   (adre.Achternaam?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false);
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddAdre>("Add Adre", null);
            await grid0.Reload();
        }

        protected async Task EditRow(KlantBaseWebDemo.Models.KlantBase.Adre args)
        {
            await DialogService.OpenAsync<EditAdre>("Edit Adre", new Dictionary<string, object> { { "Id", args.Id } });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, KlantBaseWebDemo.Models.KlantBase.Adre adre)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await KlantBaseService.DeleteAdre(adre.Id);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                        var queryableAdres = await KlantBaseService.GetAdres();
                        allAdres = await queryableAdres.ToListAsync();
                        FilterAdres();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Error",
                    Detail = "Unable to delete Adre"
                });
            }
        }
    }
}