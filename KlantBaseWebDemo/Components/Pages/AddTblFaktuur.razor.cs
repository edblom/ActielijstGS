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
    public partial class AddTblFaktuur
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

        protected override async Task OnInitializedAsync()
        {
            tblFaktuur = new KlantBaseWebDemo.Models.KlantBase.TblFaktuur();
        }
        protected bool errorVisible;
        protected KlantBaseWebDemo.Models.KlantBase.TblFaktuur tblFaktuur;

        protected async Task FormSubmit()
        {
            try
            {
                await KlantBaseService.CreateTblFaktuur(tblFaktuur);
                DialogService.Close(tblFaktuur);
            }
            catch (Exception ex)
            {
                hasChanges = ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException;
                canEdit = !(ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException);
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected bool hasChanges = false;
        protected bool canEdit = true;

        [Inject]
        protected SecurityService Security { get; set; }
    }
}