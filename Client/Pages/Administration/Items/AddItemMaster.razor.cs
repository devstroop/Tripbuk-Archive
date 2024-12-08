using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace ERP.Client.Pages.Administration.Items
{
    public partial class AddItemMaster
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
            itemMaster = new ERP.Server.Models.Postgres.ItemMaster();
        }
        protected bool errorVisible;
        protected ERP.Server.Models.Postgres.ItemMaster itemMaster;

        protected IEnumerable<ERP.Server.Models.Postgres.Master> mastersForMasterId;

        protected IEnumerable<ERP.Server.Models.Postgres.ItemGroupMaster> itemGroupMastersForGroup;


        protected int mastersForMasterIdCount;
        protected ERP.Server.Models.Postgres.Master mastersForMasterIdValue;
        protected async Task mastersForMasterIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await PostgresService.GetMasters(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                mastersForMasterId = result.Value.AsODataEnumerable();
                mastersForMasterIdCount = result.Count;

                if (!object.Equals(itemMaster.MasterId, null))
                {
                    var valueResult = await PostgresService.GetMasters(filter: $"Id eq {itemMaster.MasterId}");
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

        protected int itemGroupMastersForGroupCount;
        protected ERP.Server.Models.Postgres.ItemGroupMaster itemGroupMastersForGroupValue;
        protected async Task itemGroupMastersForGroupLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await PostgresService.GetItemGroupMasters(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(GroupName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                itemGroupMastersForGroup = result.Value.AsODataEnumerable();
                itemGroupMastersForGroupCount = result.Count;

                if (!object.Equals(itemMaster.Group, null))
                {
                    var valueResult = await PostgresService.GetItemGroupMasters(filter: $"MasterId eq {itemMaster.Group}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        itemGroupMastersForGroupValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load ItemGroupMaster" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                var result = await PostgresService.CreateItemMaster(itemMaster);
                DialogService.Close(itemMaster);
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