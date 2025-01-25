using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace Tripbuk.Client.Pages.Admin.Content.Destinations
{
    public partial class AddDestination
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
            destination = new Tripbuk.Server.Models.Postgres.Destination();
        }
        protected bool errorVisible;
        protected Tripbuk.Server.Models.Postgres.Destination destination;

        protected IEnumerable<Tripbuk.Server.Models.Postgres.Destination> destinationsForParentDestinationId;


        protected int destinationsForParentDestinationIdCount;
        protected Tripbuk.Server.Models.Postgres.Destination destinationsForParentDestinationIdValue;

        [Inject]
        protected SecurityService Security { get; set; }
        protected async Task destinationsForParentDestinationIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await PostgresService.GetDestinations(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Name, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                destinationsForParentDestinationId = result.Value.AsODataEnumerable();
                destinationsForParentDestinationIdCount = result.Count;

                if (!object.Equals(destination.ParentDestinationId, null))
                {
                    var valueResult = await PostgresService.GetDestinations(filter: $"Id eq {destination.ParentDestinationId}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        destinationsForParentDestinationIdValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Destination1" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                await PostgresService.CreateDestination(destination);
                DialogService.Close(destination);
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
    }
}