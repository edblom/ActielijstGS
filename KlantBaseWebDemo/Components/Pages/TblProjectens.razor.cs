using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace KlantBaseWebDemo.Components.Pages
{
    public partial class TblProjectens
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

        protected IEnumerable<KlantBaseWebDemo.Models.KlantBase.TblProjecten> tblProjectens;
        protected IEnumerable<KlantBaseWebDemo.Models.KlantBase.TblProjecten> filteredProjectens;
        protected RadzenDataGrid<KlantBaseWebDemo.Models.KlantBase.TblProjecten> grid0;

        [Inject]
        protected SecurityService Security { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                tblProjectens = await KlantBaseService.GetTblProjectens();
                filteredProjectens = tblProjectens;
                Console.WriteLine("OnInitializedAsync completed");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"OnInitializedAsync error: {ex.Message}");
            }
        }
        protected async Task TextBox0Change(string args)
        {
            Console.WriteLine($"Textbox={args}");
        }
        protected async Task SearchValueChanged(string args)
        {
            await FilterData(args);
        }
        protected async Task FilterData(string searchText)
        {
            Console.WriteLine($"FilterData called with searchText: '{searchText}'");
            try
            {
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    filteredProjectens = tblProjectens;
                }
                else
                {
                    string searchLower = searchText.ToLower();
                    filteredProjectens = tblProjectens.Where(p =>
                        (p.FldProjectNaam != null && p.FldProjectNaam.ToLower().Contains(searchLower)) ||
                        (p.FldAdres!= null && p.FldAdres.ToLower().Contains(searchLower)) ||
                        (p.FldExternNummer!= null && p.FldExternNummer.ToLower().Contains(searchLower)) ||
                        (p.FldPlaats!= null && p.FldPlaats.ToLower().Contains(searchLower)) ||
                        (p.FldProjectNummer.HasValue && p.FldProjectNummer.Value.ToString().Contains(searchText))
                    ).ToList();
                }
                Console.WriteLine($"Filtered {filteredProjectens.Count()} items");
                await grid0.Reload();
                StateHasChanged();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"FilterData error: {ex.Message}");
            }
        }
        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddTblProjecten>("Add TblProjecten", null);
            await grid0.Reload();
        }

        protected async Task EditRow(KlantBaseWebDemo.Models.KlantBase.TblProjecten args)
        {
            NavigationManager.NavigateTo($"/projectdetail/{args.Id}");
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, KlantBaseWebDemo.Models.KlantBase.TblProjecten tblProjecten)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await KlantBaseService.DeleteTblProjecten(tblProjecten.Id);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete TblProjecten"
                });
            }
        }

    }
}