using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace ERP.Client.Pages.Administration.Access.Users
{
    public partial class AddUser
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
        private ERP.Server.Models.ApplicationUser _user;
        private IEnumerable<string> _userRoles = [];
        private string _error;
        private bool _errorVisible;

        [Inject]
        protected SecurityService Security { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _user = new ERP.Server.Models.ApplicationUser();

            _roles = await Security.GetRoles();
        }

        private async Task FormSubmit(ERP.Server.Models.ApplicationUser user)
        {
            try
            {
                user.Roles = _roles.Where(role => _userRoles.Contains(role.Id)).ToList();
                await Security.CreateUser(user);
                DialogService.Close(null);
            }
            catch (Exception ex)
            {
                _errorVisible = true;
                _error = ex.Message;
            }
        }

        private async Task CancelClick()
        {
            DialogService.Close(null);
        }
    }
}