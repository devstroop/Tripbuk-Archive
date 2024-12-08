using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace ERP.Client.Pages.Administration.AccountGroups
{
    public partial class AddAccountGroupMaster
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
        [Inject]
        public PostgresService PostgresService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            accountGroupMaster = new ERP.Server.Models.Postgres.AccountGroupMaster();
        }
        protected bool errorVisible;
        protected ERP.Server.Models.Postgres.AccountGroupMaster accountGroupMaster;

        protected IEnumerable<ERP.Server.Models.Postgres.Master> mastersForMasterId;

        protected IEnumerable<ERP.Server.Models.Postgres.AccountGroupMaster> accountGroupMastersForParent;


        protected int mastersForMasterIdCount;
        protected ERP.Server.Models.Postgres.Master mastersForMasterIdValue;
        protected async Task mastersForMasterIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await PostgresService.GetMasters(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                mastersForMasterId = result.Value.AsODataEnumerable();
                mastersForMasterIdCount = result.Count;

                if (!object.Equals(accountGroupMaster.MasterId, null))
                {
                    var valueResult = await PostgresService.GetMasters(filter: $"Id eq {accountGroupMaster.MasterId}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        mastersForMasterIdValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Master" });
            }
        }

        protected int accountGroupMastersForParentCount;
        protected ERP.Server.Models.Postgres.AccountGroupMaster accountGroupMastersForParentValue;
        protected async Task accountGroupMastersForParentLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await PostgresService.GetAccountGroupMasters(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(GroupName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                accountGroupMastersForParent = result.Value.AsODataEnumerable();
                accountGroupMastersForParentCount = result.Count;

                if (!object.Equals(accountGroupMaster.Parent, null))
                {
                    var valueResult = await PostgresService.GetAccountGroupMasters(filter: $"MasterId eq {accountGroupMaster.Parent}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        accountGroupMastersForParentValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load AccountGroupMaster1" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                var result = await PostgresService.CreateAccountGroupMaster(accountGroupMaster);
                DialogService.Close(accountGroupMaster);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected bool hasChanges = false;
        protected bool canEdit = true;

        [Inject]
        protected SecurityService Security { get; set; }
    }
}