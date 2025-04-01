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
    public partial class VwKiwainspecties
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

        protected IEnumerable<KlantBaseWebDemo.Models.KlantBase.VwKiwainspecty> vwKiwainspecties;

        protected RadzenDataGrid<KlantBaseWebDemo.Models.KlantBase.VwKiwainspecty> grid0;

        [Inject]
        protected SecurityService Security { get; set; }
        protected override async Task OnInitializedAsync()
        {
            vwKiwainspecties = await KlantBaseService.GetVwKiwainspecties();
        }
    }
}