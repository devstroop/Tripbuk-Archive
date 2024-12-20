using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace ERP.Client.Pages.Management.Access.Roles
{
    public partial class AddRole
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

        protected ERP.Server.Models.ApplicationRole Role;
        protected string Error;
        protected bool ErrorVisible;

        [Inject]
        protected SecurityService Security { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Role = new ERP.Server.Models.ApplicationRole();
        }

        protected async Task FormSubmit(ERP.Server.Models.ApplicationRole role)
        {
            try
            {
                await Security.CreateRole(role);

                DialogService.Close(null);
            }
            catch (Exception ex)
            {
                ErrorVisible = true;
                Error = ex.Message;
            }
        }

        protected async Task CancelClick()
        {
            DialogService.Close(null);
        }
    }
}