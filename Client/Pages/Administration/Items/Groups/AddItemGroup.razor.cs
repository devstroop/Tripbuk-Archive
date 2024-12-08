using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace ERP.Client.Pages.Administration.Items.Groups
{
    public partial class AddItemGroup
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
            ItemGroup = new ERP.Server.Models.Postgres.ItemGroup();
        }
        protected bool ErrorVisible;
        protected ERP.Server.Models.Postgres.ItemGroup ItemGroup;

        protected IEnumerable<ERP.Server.Models.Postgres.ItemGroup> ItemGroupsForParent;


        protected int ItemGroupsForParentCount;
        protected ERP.Server.Models.Postgres.ItemGroup ItemGroupsForParentValue;
        protected async Task ItemGroupsForParentLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await PostgresService.GetItemGroups(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(GroupName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                ItemGroupsForParent = result.Value.AsODataEnumerable();
                ItemGroupsForParentCount = result.Count;

                if (!object.Equals(ItemGroup.Parent, null))
                {
                    var valueResult = await PostgresService.GetItemGroups(filter: $"Id eq {ItemGroup.Parent}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        ItemGroupsForParentValue = firstItem;
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
                var result = await PostgresService.CreateItemGroup(ItemGroup);
                DialogService.Close(ItemGroup);
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