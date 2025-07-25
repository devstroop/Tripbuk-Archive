using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace Tripbuk.Client.Pages.Admin.Security.Tenants
{
    public partial class EditTenant
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

        protected IEnumerable<Tripbuk.Server.Models.ApplicationRole> roles;
        protected Tripbuk.Server.Models.ApplicationTenant tenant;
        protected string error;
        protected bool errorVisible;

        [Parameter]
        public int Id { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        protected override async Task OnInitializedAsync()
        {
            tenant = await Security.GetTenantById(Id);
        }

        protected async Task FormSubmit(Tripbuk.Server.Models.ApplicationTenant tenant)
        {
            try
            {
                await Security.UpdateTenant(Id, tenant);
                DialogService.Close(null);
            }
            catch (Exception ex)
            {
                errorVisible = true;
                error = ex.Message;
            }
        }

        protected async Task CancelClick()
        {
            DialogService.Close(null);
        }
    }
}