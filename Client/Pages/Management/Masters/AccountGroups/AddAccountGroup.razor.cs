using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace ERP.Client.Pages.Management.Masters.AccountGroups
{
    public partial class AddAccountGroup
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
            AccountGroup = new ERP.Server.Models.Postgres.AccountGroup();
        }
        protected bool ErrorVisible;
        protected ERP.Server.Models.Postgres.AccountGroup AccountGroup;

        protected IEnumerable<ERP.Server.Models.Postgres.AccountGroup> AccountGroupsForParent;


        protected int AccountGroupsForParentCount;
        protected ERP.Server.Models.Postgres.AccountGroup AccountGroupsForParentValue;
        protected async Task AccountGroupsForParentLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await PostgresService.GetAccountGroups(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(GroupName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                AccountGroupsForParent = result.Value.AsODataEnumerable();
                AccountGroupsForParentCount = result.Count;

                if (!object.Equals(AccountGroup.Parent, null))
                {
                    var valueResult = await PostgresService.GetAccountGroups(filter: $"Id eq {AccountGroup.Parent}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        AccountGroupsForParentValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load AccountGroup1" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                var result = await PostgresService.CreateAccountGroup(AccountGroup);
                DialogService.Close(AccountGroup);
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