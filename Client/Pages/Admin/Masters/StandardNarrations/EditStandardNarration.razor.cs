using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace Tripbuk.Client.Pages.Admin.Masters.StandardNarrations
{
    public partial class EditStandardNarration
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

        [Parameter]
        public int Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            StandardNarration = await PostgresService.GetStandardNarrationById(id:Id);
        }
        protected bool ErrorVisible;
        protected Tripbuk.Server.Models.Postgres.StandardNarration StandardNarration;

        protected async Task FormSubmit()
        {
            try
            {
                var result = await PostgresService.UpdateStandardNarration(id:Id, StandardNarration);
                if (result.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                {
                     HasChanges = true;
                     CanEdit = false;
                     return;
                }
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


        protected async Task ReloadButtonClick(MouseEventArgs args)
        {
            HasChanges = false;
            CanEdit = true;

            StandardNarration = await PostgresService.GetStandardNarrationById(id:Id);
        }
    }
}