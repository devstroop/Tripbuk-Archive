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
    public partial class AddItemGroup
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
            itemGroup = new ERP.Server.Models.Postgres.ItemGroup();
        }
        protected bool errorVisible;
        protected ERP.Server.Models.Postgres.ItemGroup itemGroup;

        protected IEnumerable<ERP.Server.Models.Postgres.ItemGroup> itemGroupsForParent;


        protected int itemGroupsForParentCount;
        protected ERP.Server.Models.Postgres.ItemGroup itemGroupsForParentValue;
        protected async Task itemGroupsForParentLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await PostgresService.GetItemGroups(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(GroupName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                itemGroupsForParent = result.Value.AsODataEnumerable();
                itemGroupsForParentCount = result.Count;

                if (!object.Equals(itemGroup.Parent, null))
                {
                    var valueResult = await PostgresService.GetItemGroups(filter: $"Id eq {itemGroup.Parent}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        itemGroupsForParentValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load ItemGroup1" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                var result = await PostgresService.CreateItemGroup(itemGroup);
                DialogService.Close(itemGroup);
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