using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace ERP.Client.Pages.Administration.StdNarrations
{
    public partial class AddStdNarrationMaster
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
            stdNarrationMaster = new ERP.Server.Models.Postgres.StdNarrationMaster();
        }
        protected bool errorVisible;
        protected ERP.Server.Models.Postgres.StdNarrationMaster stdNarrationMaster;

        protected IEnumerable<ERP.Server.Models.Postgres.Master> mastersForMasterId;


        protected int mastersForMasterIdCount;
        protected ERP.Server.Models.Postgres.Master mastersForMasterIdValue;
        protected async Task mastersForMasterIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await PostgresService.GetMasters(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                mastersForMasterId = result.Value.AsODataEnumerable();
                mastersForMasterIdCount = result.Count;

                if (!object.Equals(stdNarrationMaster.MasterId, null))
                {
                    var valueResult = await PostgresService.GetMasters(filter: $"Id eq {stdNarrationMaster.MasterId}");
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
        protected async Task FormSubmit()
        {
            try
            {
                var result = await PostgresService.CreateStdNarrationMaster(stdNarrationMaster);
                DialogService.Close(stdNarrationMaster);
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