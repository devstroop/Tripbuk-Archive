using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace ERP.Client.Pages.Administration.Masters.StandardNarrations
{
    public partial class StandardNarrations
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

        private IEnumerable<ERP.Server.Models.Postgres.StandardNarration> _standardNarrations;

        private RadzenDataGrid<ERP.Server.Models.Postgres.StandardNarration> _grid0;
        private int _count;

        private string _search = "";

        [Inject]
        protected SecurityService Security { get; set; }

        protected async Task Search(ChangeEventArgs args)
        {
            _search = $"{args.Value}";

            await _grid0.GoToPage(0);

            await _grid0.Reload();
        }

        private async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var result = await PostgresService.GetStandardNarrations(filter: $@"(contains(Narration,""{_search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                _standardNarrations = result.Value.AsODataEnumerable();
                _count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Standard Narrations" });
            }
        }

        private async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddStandardNarration>("Add Standard Narration", null);
            await _grid0.Reload();
        }

        private async Task EditRow(ERP.Server.Models.Postgres.StandardNarration args)
        {
            await DialogService.OpenAsync<EditStandardNarration>("Edit Standard Narration", new Dictionary<string, object> { {"Id", args.Id} });
            await _grid0.Reload();
        }

        private async Task GridDeleteButtonClick(MouseEventArgs args, ERP.Server.Models.Postgres.StandardNarration standardNarration)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await PostgresService.DeleteStandardNarration(id:standardNarration.Id);

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
                    Detail = $"Unable to delete StandardNarration"
                });
            }
        }
    }
}