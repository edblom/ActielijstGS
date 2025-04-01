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
    public partial class TblMemos
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

        protected IEnumerable<KlantBaseWebDemo.Models.KlantBase.TblMemo> tblMemos;

        protected RadzenDataGrid<KlantBaseWebDemo.Models.KlantBase.TblMemo> grid0;

        [Inject]
        protected SecurityService Security { get; set; }
        protected override async Task OnInitializedAsync()
        {
            tblMemos = await KlantBaseService.GetTblMemos();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddTblMemo>("Add TblMemo", null);
            await grid0.Reload();
        }

        protected async Task EditRow(KlantBaseWebDemo.Models.KlantBase.TblMemo args)
        {
            await DialogService.OpenAsync<EditTblMemo>("Edit TblMemo", new Dictionary<string, object> { {"FldMid", args.FldMid} });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, KlantBaseWebDemo.Models.KlantBase.TblMemo tblMemo)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await KlantBaseService.DeleteTblMemo(tblMemo.FldMid);

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
                    Detail = $"Unable to delete TblMemo"
                });
            }
        }
    }
}