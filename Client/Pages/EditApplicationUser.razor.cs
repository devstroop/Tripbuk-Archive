using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace ERP.Client.Pages
{
    public partial class EditApplicationUser
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

        protected IEnumerable<ERP.Server.Models.ApplicationRole> Roles;
        protected ERP.Server.Models.ApplicationUser User;
        protected IEnumerable<string> UserRoles;
        protected string Error;
        protected bool ErrorVisible;

        [Parameter]
        public string Id { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        protected override async Task OnInitializedAsync()
        {
            User = await Security.GetUserById($"{Id}");

            UserRoles = User.Roles.Select(role => role.Id);

            Roles = await Security.GetRoles();
        }

        protected async Task FormSubmit(ERP.Server.Models.ApplicationUser user)
        {
            try
            {
                user.Roles = Roles.Where(role => UserRoles.Contains(role.Id)).ToList();
                await Security.UpdateUser($"{Id}", user);
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