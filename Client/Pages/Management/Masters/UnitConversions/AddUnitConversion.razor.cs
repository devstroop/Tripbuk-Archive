using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace ERP.Client.Pages.Management.Masters.UnitConversions
{
    public partial class AddUnitConversion
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
            _unitConversion = new ERP.Server.Models.Postgres.UnitConversion();
        }

        private bool _errorVisible;
        private ERP.Server.Models.Postgres.UnitConversion _unitConversion;

        private IEnumerable<ERP.Server.Models.Postgres.Unit> _unitsForMainUnit;
        private int _unitsForMainUnitCount;
        private ERP.Server.Models.Postgres.Unit _unitsForMainUnitValue;

        private async Task UnitsForMainUnitLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await PostgresService.GetUnits(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(UnitName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                _unitsForMainUnit = result.Value.AsODataEnumerable();
                _unitsForMainUnitCount = result.Count;

                if (!object.Equals(_unitConversion.MainUnit, null))
                {
                    var valueResult = await PostgresService.GetUnits(filter: $"Id eq {_unitConversion.MainUnit}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        _unitsForMainUnitValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Unit" });
            }
        }


        private IEnumerable<ERP.Server.Models.Postgres.Unit> _unitsForSubUnit;
        private int _unitsForSubUnitCount;
        private ERP.Server.Models.Postgres.Unit _unitsForSubUnitValue;

        private async Task UnitsForSubUnitLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await PostgresService.GetUnits(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(UnitName, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                _unitsForSubUnit = result.Value.AsODataEnumerable();
                _unitsForSubUnitCount = result.Count;

                if (!object.Equals(_unitConversion.SubUnit, null))
                {
                    var valueResult = await PostgresService.GetUnits(filter: $"Id eq {_unitConversion.SubUnit}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        _unitsForSubUnitValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Unit" });
            }
        }

        private async Task FormSubmit()
        {
            try
            {
                var result = await PostgresService.CreateUnitConversion(_unitConversion);
                DialogService.Close(_unitConversion);
            }
            catch (Exception ex)
            {
                _errorVisible = true;
            }
        }

        private async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }


        protected bool HasChanges = false;
        private const bool CanEdit = true;

        [Inject]
        protected SecurityService Security { get; set; }
    }
}