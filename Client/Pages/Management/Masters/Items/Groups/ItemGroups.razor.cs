using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace ERP.Client.Pages.Management.Masters.Items.Groups
{
    public partial class ItemGroups
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

        private IEnumerable<ERP.Server.Models.Postgres.ItemGroup> _itemGroups;

        private RadzenDataGrid<ERP.Server.Models.Postgres.ItemGroup> _grid0;
        private int _count;

        private string _search = "";

        [Inject]
        protected SecurityService Security { get; set; }

        private async Task Search(ChangeEventArgs args)
        {
            _search = $"{args.Value}";

            await _grid0.GoToPage(0);

            await _grid0.Reload();
        }

        private async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var result = await PostgresService.GetItemGroups(filter: $@"(contains(GroupName,""{_search}"") or contains(Alias,""{_search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", expand: "ItemGroup1", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                _itemGroups = result.Value.AsODataEnumerable();
                _count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load ItemGroups" });
            }
        }

        private async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<CreateItemGroup>("Add ItemGroup", null);
            await _grid0.Reload();
        }

        protected async Task EditRow(ERP.Server.Models.Postgres.ItemGroup args)
        {
            await DialogService.OpenAsync<EditItemGroup>("Edit ItemGroup", new Dictionary<string, object> { {"Id", args.Id} });
            await _grid0.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, ERP.Server.Models.Postgres.ItemGroup itemGroup)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await PostgresService.DeleteItemGroup(id:itemGroup.Id);

                    if (deleteResult != null)
                    {
                        await _grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete ItemGroup"
                });
            }
        }
    }
}