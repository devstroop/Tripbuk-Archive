using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace Tripbuk.Client.Pages.Admin.Masters.Units
{
    public partial class Units
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

        private IEnumerable<Tripbuk.Server.Models.Postgres.Unit> _units;

        private RadzenDataGrid<Tripbuk.Server.Models.Postgres.Unit> _grid0;
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
                var result = await PostgresService.GetUnits(filter: $@"(contains(UnitName,""{_search}"") or contains(Alias,""{_search}"") or contains(PrintName,""{_search}"") or contains(Uqc,""{_search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                _units = result.Value.AsODataEnumerable();
                _count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Units" });
            }
        }

        private async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddUnit>("Add Unit", null);
            await _grid0.Reload();
        }

        private async Task EditRow(Tripbuk.Server.Models.Postgres.Unit args)
        {
            await DialogService.OpenAsync<EditUnit>("Edit Unit", new Dictionary<string, object> { {"Id", args.Id} });
            await _grid0.Reload();
        }

        private async Task GridDeleteButtonClick(MouseEventArgs args, Tripbuk.Server.Models.Postgres.Unit unit)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await PostgresService.DeleteUnit(id:unit.Id);

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
                    Detail = $"Unable to delete Unit"
                });
            }
        }
    }
}