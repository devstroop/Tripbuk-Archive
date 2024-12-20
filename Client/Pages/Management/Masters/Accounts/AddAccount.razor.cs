using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace ERP.Client.Pages.Management.Masters.Accounts
{
    public partial class AddAccount
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
            Account = new ERP.Server.Models.Postgres.Account();
        }
        protected bool ErrorVisible;
        protected ERP.Server.Models.Postgres.Account Account;

        protected IEnumerable<ERP.Server.Models.Postgres.AccountGroup> AccountGroupsForGroup;


        protected int AccountGroupsForGroupCount;
        protected ERP.Server.Models.Postgres.AccountGroup AccountGroupsForGroupValue;
        protected async Task AccountGroupsForGroupLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await PostgresService.GetAccountGroups(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(GroupName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                AccountGroupsForGroup = result.Value.AsODataEnumerable();
                AccountGroupsForGroupCount = result.Count;

                if (!object.Equals(Account.Group, null))
                {
                    var valueResult = await PostgresService.GetAccountGroups(filter: $"Id eq {Account.Group}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        AccountGroupsForGroupValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load AccountGroup" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                var result = await PostgresService.CreateAccount(Account);
                DialogService.Close(Account);
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