using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace ERP.Client.Pages.Administration.Accounts
{
    public partial class EditAccountMaster
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

        [Parameter]
        public int MasterId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            accountMaster = await PostgresService.GetAccountMasterByMasterId(masterId:MasterId);
        }
        protected bool errorVisible;
        protected ERP.Server.Models.Postgres.AccountMaster accountMaster;

        protected IEnumerable<ERP.Server.Models.Postgres.Master> mastersForMasterId;

        protected IEnumerable<ERP.Server.Models.Postgres.AccountGroupMaster> accountGroupMastersForGroup;


        protected int mastersForMasterIdCount;
        protected ERP.Server.Models.Postgres.Master mastersForMasterIdValue;
        protected async Task mastersForMasterIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await PostgresService.GetMasters(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                mastersForMasterId = result.Value.AsODataEnumerable();
                mastersForMasterIdCount = result.Count;

                if (!object.Equals(accountMaster.MasterId, null))
                {
                    var valueResult = await PostgresService.GetMasters(filter: $"Id eq {accountMaster.MasterId}");
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

        protected int accountGroupMastersForGroupCount;
        protected ERP.Server.Models.Postgres.AccountGroupMaster accountGroupMastersForGroupValue;
        protected async Task accountGroupMastersForGroupLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await PostgresService.GetAccountGroupMasters(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(GroupName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                accountGroupMastersForGroup = result.Value.AsODataEnumerable();
                accountGroupMastersForGroupCount = result.Count;

                if (!object.Equals(accountMaster.Group, null))
                {
                    var valueResult = await PostgresService.GetAccountGroupMasters(filter: $"MasterId eq {accountMaster.Group}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        accountGroupMastersForGroupValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load AccountGroupMaster" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                var result = await PostgresService.UpdateAccountMaster(masterId:MasterId, accountMaster);
                if (result.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                {
                     hasChanges = true;
                     canEdit = false;
                     return;
                }
                DialogService.Close(accountMaster);
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


        protected async Task ReloadButtonClick(MouseEventArgs args)
        {
            hasChanges = false;
            canEdit = true;

            accountMaster = await PostgresService.GetAccountMasterByMasterId(masterId:MasterId);
        }
    }
}