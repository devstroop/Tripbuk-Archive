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
    public partial class Tenants
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

        protected IEnumerable<Tripbuk.Server.Models.ApplicationTenant> tenants;
        protected RadzenDataGrid<Tripbuk.Server.Models.ApplicationTenant> grid0;
        protected string error;
        protected bool errorVisible;

        [Inject]
        protected SecurityService Security { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }

        protected async Task Load()
        {
            tenants = await Security.GetTenants();

            if (Security.Tenant == null && tenants != null && tenants.Any()) {
                Security.Tenant = tenants.FirstOrDefault();
            }
        }

        protected async Task AddClick()
        {
            await DialogService.OpenAsync<AddTenant>("Add Application Tenant");

            await Load();
        }

        protected async Task SetDefaultTenant(MouseEventArgs args, Tripbuk.Server.Models.ApplicationTenant tenant)
        {
            Security.Tenant = tenant;
        }

        protected async Task RowSelect(Tripbuk.Server.Models.ApplicationTenant tenant)
        {
            await DialogService.OpenAsync<EditTenant>("Edit Application Tenant", new Dictionary<string, object>{ {"Id", tenant.Id} });

            await Load();
        }

        protected async void RowRender(RowRenderEventArgs<Tripbuk.Server.Models.ApplicationTenant> args)
        {
            args.Attributes.Add("style", $"font-weight: {(args.Data.Id == Security.Tenant?.Id ? "bold" : "normal")};");
        }

        protected async Task DeleteClick(Tripbuk.Server.Models.ApplicationTenant tenant)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this tenant?") == true)
                {
                    await Security.DeleteTenant(tenant.Id);

                    await Load();
                }
            }
            catch (Exception ex)
            {
                errorVisible = true;
                error = ex.Message;
            }
        }
    }
}