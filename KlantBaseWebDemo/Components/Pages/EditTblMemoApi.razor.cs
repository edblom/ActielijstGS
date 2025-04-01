using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using System;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace KlantBaseWebDemo.Components.Pages
{
    [Authorize]
    public partial class EditTblMemoApi : ComponentBase
    {
        [Inject] protected IHttpClientFactory HttpClientFactory { get; set; }
        [Inject] protected DialogService DialogService { get; set; }
        [Inject] protected NotificationService NotificationService { get; set; }

        [Parameter]
        public int FldMid { get; set; }

        protected KlantBaseWebDemo.Models.KlantBase.TblMemo tblMemo;
        protected bool errorVisible;
        private HttpClient Http => HttpClientFactory.CreateClient("KlantBaseApi");

        protected override async Task OnInitializedAsync()
        {
            try
            {
                tblMemo = await Http.GetFromJsonAsync<KlantBaseWebDemo.Models.KlantBase.TblMemo>($"api/acties/{FldMid}");
                if (tblMemo == null)
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Error,
                        Summary = "Fout",
                        Detail = "Actie niet gevonden."
                    });
                    DialogService.Close(null);
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
                DialogService.Close(null);
            }
        }

        protected async Task FormSubmit()
        {
            try
            {
                var response = await Http.PutAsJsonAsync($"api/acties/{FldMid}", tblMemo);
                if (response.IsSuccessStatusCode)
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Success,
                        Summary = "Opgeslagen",
                        Detail = "Actie succesvol bijgewerkt."
                    });
                    DialogService.Close(tblMemo);
                }
                else
                {
                    errorVisible = true;
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Error,
                        Summary = "Fout bij opslaan",
                        Detail = $"Status: {response.StatusCode}"
                    });
                }
            }
            catch (Exception ex)
            {
                errorVisible = true;
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = "Fout bij opslaan",
                    Detail = ex.Message
                });
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}