using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace ERP.Client.Pages.Administration.StandardNarrations
{
    public partial class AddStandardNarration
    {
        [Inject]
        protected IJSRuntime JsRuntime { get; set; }

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
        public PostgresService PostgresService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            StandardNarration = new ERP.Server.Models.Postgres.StandardNarration();
        }
        protected bool ErrorVisible;
        protected ERP.Server.Models.Postgres.StandardNarration StandardNarration;

        protected async Task FormSubmit()
        {
            try
            {
                var result = await PostgresService.CreateStandardNarration(StandardNarration);
                DialogService.Close(StandardNarration);
            }
            catch (Exception ex)
            {
                ErrorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected bool HasChanges = false;
        protected bool CanEdit = true;

        [Inject]
        protected SecurityService Security { get; set; }
    }
}