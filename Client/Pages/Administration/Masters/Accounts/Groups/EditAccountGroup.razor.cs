using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace ERP.Client.Pages.Administration.Masters.Accounts.Groups
{
    public partial class EditAccountGroup
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
            AccountGroup = await PostgresService.GetAccountGroupById(id:Id);
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
                var result = await PostgresService.UpdateAccountGroup(id:Id, AccountGroup);
                if (result.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                {
                     HasChanges = true;
                     CanEdit = false;
                     return;
                }
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


        protected async Task ReloadButtonClick(MouseEventArgs args)
        {
            HasChanges = false;
            CanEdit = true;

            AccountGroup = await PostgresService.GetAccountGroupById(id:Id);
        }

        private async Task DeleteClick(MouseEventArgs arg)
        {
            if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
            {
                var deleteResult = await PostgresService.DeleteAccountGroup(id:AccountGroup.Id);

                if (deleteResult != null)
                {
                    DialogService.Close(null);
                }
            }
        }
    }
}