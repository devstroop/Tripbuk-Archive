using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace ERP.Client.Pages.Management.Access.Roles
{
    public partial class Roles
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

        private IEnumerable<ERP.Server.Models.ApplicationRole> _roles;
        private RadzenDataGrid<ERP.Server.Models.ApplicationRole> _grid0;
        private string _error;
        private bool _errorVisible;

        [Inject]
        protected SecurityService Security { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _roles = await Security.GetRoles();
        }

        private async Task AddClick()
        {
            await DialogService.OpenAsync<AddRole>("Add Role");

            _roles = await Security.GetRoles();
        }

        private async Task DeleteClick(ERP.Server.Models.ApplicationRole role)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this role?") == true)
                {
                    await Security.DeleteRole($"{role.Id}");

                    _roles = await Security.GetRoles();
                }
            }
            catch (Exception ex)
            {
                _errorVisible = true;
                _error = ex.Message;
            }
        }
    }
}