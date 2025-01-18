using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace ERP.Client.Pages.Admin.Application.Users
{
    public partial class Users
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

        private IEnumerable<ERP.Server.Models.ApplicationUser> _users;
        protected RadzenDataGrid<ERP.Server.Models.ApplicationUser> Grid0;
        private string _error;
        private bool _errorVisible;

        [Inject]
        protected SecurityService Security { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _users = await Security.GetUsers();
        }

        private async Task AddClick()
        {
            await DialogService.OpenAsync<AddUser>("Add User");

            _users = await Security.GetUsers();
        }

        private async Task RowSelect(ERP.Server.Models.ApplicationUser user)
        {
            await DialogService.OpenAsync<EditUser>("Edit User", new Dictionary<string, object>{ {"Id", user.Id} });

            _users = await Security.GetUsers();
        }

        private async Task DeleteClick(ERP.Server.Models.ApplicationUser user)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this user?") == true)
                {
                    await Security.DeleteUser($"{user.Id}");

                    _users = await Security.GetUsers();
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