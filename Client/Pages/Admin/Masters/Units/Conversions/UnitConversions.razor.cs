using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace TripBUK.Client.Pages.Admin.Masters.Units.Conversions
{
    public partial class UnitConversions
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

        private IEnumerable<TripBUK.Server.Models.Postgres.UnitConversion> _unitConversions;

        private RadzenDataGrid<TripBUK.Server.Models.Postgres.UnitConversion> _grid0;
        private int _count;

        protected string search = "";

        [Inject]
        protected SecurityService Security { get; set; }

        private async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await _grid0.GoToPage(0);

            await _grid0.Reload();
        }

        private async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var result = await PostgresService.GetUnitConversions(filter: $"{args.Filter}", expand: "Unit", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                _unitConversions = result.Value.AsODataEnumerable();
                _count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load UnitConversions" });
            }
        }

        private async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<CreateUnitConversion>("Add Unit Conversion", null);
            await _grid0.Reload();
        }

        private async Task EditRow(TripBUK.Server.Models.Postgres.UnitConversion args)
        {
            await DialogService.OpenAsync<EditUnitConversion>("Edit Unit Conversion", new Dictionary<string, object> { {"Id", args.Id} });
            await _grid0.Reload();
        }

        private async Task GridDeleteButtonClick(MouseEventArgs args, TripBUK.Server.Models.Postgres.UnitConversion unitConversion)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await PostgresService.DeleteUnitConversion(id:unitConversion.Id);

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
                    Detail = $"Unable to delete UnitConversion"
                });
            }
        }
    }
}