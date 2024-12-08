using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace ERP.Client.Pages.Administration.Masters.StandardNarration
{
    public partial class AddStandardNarration
    public partial class AddStandardNarration : ComponentBase
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
            _standardNarration = new ERP.Server.Models.Postgres.StandardNarration();
        }

        private bool _errorVisible;
        private ERP.Server.Models.Postgres.StandardNarration _standardNarration;

        private async Task FormSubmit()
        {
            try
            {
                var result = await PostgresService.CreateStandardNarration(_standardNarration);
                DialogService.Close(_standardNarration);
            }
            catch (Exception ex)
            {
                _errorVisible = true;
            }
        }

        private async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected bool HasChanges = false;
        private const bool CanEdit = true;

        [Inject]
        protected SecurityService Security { get; set; }
    }
}