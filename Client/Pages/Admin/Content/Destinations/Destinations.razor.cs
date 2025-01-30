using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using Tripbuk.Client.Components;
using Tripbuk.Client.Components.ImportWizard;
using Tripbuk.Client.Services;
using Tripbuk.Server.Models.Postgres;

namespace Tripbuk.Client.Pages.Admin.Content.Destinations
{
    public partial class Destinations
    {
        [Inject] protected IJSRuntime JSRuntime { get; set; }

        [Inject] protected NavigationManager NavigationManager { get; set; }

        [Inject] protected DialogService DialogService { get; set; }

        [Inject] protected TooltipService TooltipService { get; set; }

        [Inject] protected ContextMenuService ContextMenuService { get; set; }

        [Inject] protected NotificationService NotificationService { get; set; }

        [Inject] public PostgresService PostgresService { get; set; }

        [Inject] public ViatorService ViatorService { get; set; }

        protected IEnumerable<Tripbuk.Server.Models.Postgres.Destination> destinations;

        protected RadzenDataGrid<Tripbuk.Server.Models.Postgres.Destination> grid0;
        protected int count;

        protected string search = "";

        [Inject] protected SecurityService Security { get; set; }
        protected ProgressWindow Prog;
        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            await grid0.Reload();
        }

        protected async Task Grid0LoadData(LoadDataArgs args)
        {
            try
            {
                var result = await PostgresService.GetDestinations(
                    filter:
                    $@"(contains(Name,""{search}"") or contains(Type,""{search}"") or contains(LookupId,""{search}"") or contains(DestinationUrl,""{search}"") or contains(DefaultCurrencyCode,""{search}"") or contains(TimeZone,""{search}"") or contains(CountryCallingCode,""{search}"")) and {(string.IsNullOrEmpty(args.Filter) ? "true" : args.Filter)}",
                    expand: "", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip,
                    count: args.Top != null && args.Skip != null);
                destinations = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Destinations"
                });
            }
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddDestination>("Add Destination", null);
            await grid0.Reload();
        }

        protected async Task EditRow(Tripbuk.Server.Models.Postgres.Destination args)
        {
            await DialogService.OpenAsync<EditDestination>("Edit Destination",
                new Dictionary<string, object> { { "Id", args.Id } });
            await grid0.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args,
            Tripbuk.Server.Models.Postgres.Destination destination)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await PostgresService.DeleteDestination(id: destination.Id);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete Destination"
                });
            }
        }

        private async Task ImportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == null)
            {
                await DialogService.OpenAsync<ImportWizard>("Import Destinations", new Dictionary<string, object>(), new DialogOptions()
                {
                    Width = "800px",
                    Height = "600px",
                    Resizable = true,
                    Draggable = true
                });
                // await Sync();
                return;
            }

            switch (args.Value)
            {
                case "import-json":
                    await ImportJson();
                    break;
                case "export-json":
                    await ExportJson();
                    break;
                case "export-xlsx":
                    await ExportXlsx();
                    break;
                case "export-csv":
                    await ExportCsv();
                    break;
                    
            }
            // if (args?.Value == "csv")
            // {
            //     await PostgresService.ExportDestinationsToCSV(new Query
            //     {
            //         Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
            //         OrderBy = $"{grid0.Query.OrderBy}",
            //         Expand = "Destination1",
            //         Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
            //     }, "Destinations");
            // }
            //
            // if (args == null || args.Value == "xlsx")
            // {
            //     await PostgresService.ExportDestinationsToExcel(new Query
            //     {
            //         Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter)? "true" : grid0.Query.Filter)}",
            //         OrderBy = $"{grid0.Query.OrderBy}",
            //         Expand = "Destination1",
            //         Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property.Contains(".") ? c.Property + " as " + c.Property.Replace(".", "") : c.Property))
            //     }, "Destinations");
            // }
        }

        private async Task ExportCsv()
        {
            throw new NotImplementedException();
        }

        private async Task ExportXlsx()
        {
            throw new NotImplementedException();
        }

        private async Task ExportJson()
        {
            throw new NotImplementedException();
        }

        private async Task ImportJson()
        {
            throw new NotImplementedException();
        }

        private async Task Sync()
        {
            var result = await DialogService.Confirm("Are you sure you want to sync destinations?",
                "Sync Destinations from Viator",
                new ConfirmOptions() { OkButtonText = "Yes", CancelButtonText = "No" });
            if (result == true)
            {
                try
                {
                    await Prog.OpenAsync("Syncing Destinations", "Please wait...");
                    
                    var destinationsResponse = await ViatorService.GetDestinations();
                    var sortedDestinations = destinationsResponse.Destinations.OrderBy(d => d.ParentDestinationId == null).ToList();
                        foreach (var destination in sortedDestinations)
                        {
                            try
                            {
                                Prog.UpdateMessage($"Adding destination: {destination.Name} ({destination.Type})");
                                var destinationEntity = new Tripbuk.Server.Models.Postgres.Destination()
                                {
                                    Id = destination.DestinationId,
                                    Name = destination.Name,
                                    Type = destination.Type,
                                    ParentDestinationId = destination.ParentDestinationId,
                                    LookupId = destination.LookupId,
                                    DestinationUrl = destination.DestinationUrl,
                                    DefaultCurrencyCode = destination.DefaultCurrencyCode,
                                    TimeZone = destination.TimeZone,
                                    Iatacodes = destination.IataCodes?.ToList(),
                                    CountryCallingCode = destination.CountryCallingCode,
                                    LocationCenter = destination.Center != null ? new LocationCenter()
                                    {
                                        Latitude = destination.Center.Latitude,
                                        Longitude = destination.Center.Longitude
                                    } : null,
                                };
                                var createdDestination = await PostgresService.CreateDestination(destinationEntity);
                                Prog.UpdateMessage($"Added destination: {destination.Name}");
                            }
                            catch (Exception ex)
                            {
                                Prog.UpdateMessage($"Error adding destination {destination.Name}: {ex.Message}");
                                await Prog.Close();
                                await DialogService.Alert($"Error adding destination {destination.Name}: {ex.Message}", "Error", new AlertOptions() { OkButtonText = "OK" });
                                break;
                            }
                        }

                        await Prog.Close();
                        await grid0.Reload();
                }
                catch (Exception ex)
                {
                    Prog.UpdateMessage($"Error migrating destinations: {ex.Message}");
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Error,
                        Summary = $"Error",
                        Detail = $"Unable to migrate destinations"
                    });
                    
                    await Prog.Close();
                    await DialogService.Alert(ex.Message, "Error", new AlertOptions() { OkButtonText = "OK" });
                }
            }
        }
    }
}