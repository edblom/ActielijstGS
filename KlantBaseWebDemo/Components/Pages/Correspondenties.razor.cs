using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace KlantBaseWebDemo.Components.Pages
{
    public partial class Correspondenties
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

        [Inject]
        protected HttpClient Http { get; set; }

        protected IEnumerable<KlantBaseWebDemo.Models.KlantBase.Correspondentie> correspondenties;

        protected RadzenDataGrid<KlantBaseWebDemo.Models.KlantBase.Correspondentie> grid0;

        protected override async Task OnInitializedAsync()
        {
            correspondenties = await KlantBaseService.GetCorrespondenties();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddCorrespondentie>("Add Correspondentie", null);
            await grid0.Reload();
        }

        protected async Task EditRow(KlantBaseWebDemo.Models.KlantBase.Correspondentie args)
        {
            await DialogService.OpenAsync<EditCorrespondentie>("Edit Correspondentie", new Dictionary<string, object> { { "Id", args.Id } });
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, KlantBaseWebDemo.Models.KlantBase.Correspondentie correspondentie)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await KlantBaseService.DeleteCorrespondentie(correspondentie.Id);

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
                    Detail = $"Unable to delete Correspondentie"
                });
            }
        }

        protected async Task OpenDocumentByCorrespondence(int correspondentieId)
        {
            try
            {
                var request = new OpenDocumentByCorrespondenceRequest { CorrespondentieId = correspondentieId };
                var response = await Http.PostAsJsonAsync("/api/Correspondence/open/by-correspondence", request);
                response.EnsureSuccessStatusCode();

                var fileName = response.Content.Headers.ContentDisposition?.FileName ?? $"correspondentie_{correspondentieId}.docx";
                var stream = await response.Content.ReadAsStreamAsync();
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                var bytes = ms.ToArray();

                await JSRuntime.InvokeVoidAsync("downloadFile", fileName, Convert.ToBase64String(bytes), "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Success,
                    Summary = "Succes",
                    Detail = "Document wordt gedownload."
                });
            }
            catch (HttpRequestException ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Fout",
                    Detail = ex.Message
                });
            }
        }

        protected class OpenDocumentByCorrespondenceRequest
        {
            public int CorrespondentieId { get; set; }
        }
    }
}