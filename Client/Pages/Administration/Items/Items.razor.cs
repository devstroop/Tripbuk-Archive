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
    public partial class Items
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

        private IEnumerable<ERP.Server.Models.Postgres.Item> _items;

        private RadzenDataGrid<ERP.Server.Models.Postgres.Item> _grid0;
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

        protected async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var result = await PostgresService.GetItems(filter: $@"(contains(ItemName,""{_search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", expand: "ItemGroup", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                _items = result.Value.AsODataEnumerable();
                _count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Items" });
            }
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddItem>("Add Item", null);
            await _grid0.Reload();
        }

        protected async Task EditRow(ERP.Server.Models.Postgres.Item args)
        {
            await DialogService.OpenAsync<EditItem>("Edit Item", new Dictionary<string, object> { {"Id", args.Id} });
            await _grid0.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, ERP.Server.Models.Postgres.Item item)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await PostgresService.DeleteItem(id:item.Id);

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
                    Detail = $"Unable to delete Item"
                });
            }
        }
    }
}