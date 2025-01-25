using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace Tripbuk.Client.Pages.Admin.Content.Places
{
    public partial class AddPlace
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
            place = new Tripbuk.Server.Models.Postgres.Place();
        }
        protected bool errorVisible;
        protected Tripbuk.Server.Models.Postgres.Place place;

        protected IEnumerable<Tripbuk.Server.Models.Postgres.Destination> destinationsForDestinationId;


        protected int destinationsForDestinationIdCount;
        protected Tripbuk.Server.Models.Postgres.Destination destinationsForDestinationIdValue;

        [Inject]
        protected SecurityService Security { get; set; }
        protected async Task destinationsForDestinationIdLoadData(LoadDataArgs args)
        {
            try
            {
                var result = await PostgresService.GetDestinations(top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null, filter: $"contains(Name, '{(!string.IsNullOrEmpty(args.Filter) ? args.Filter : "")}')", orderby: $"{args.OrderBy}");
                destinationsForDestinationId = result.Value.AsODataEnumerable();
                destinationsForDestinationIdCount = result.Count;

                if (!object.Equals(place.DestinationId, null))
                {
                    var valueResult = await PostgresService.GetDestinations(filter: $"Id eq {place.DestinationId}");
                    var firstItem = valueResult.Value.FirstOrDefault();
                    if (firstItem != null)
                    {
                        destinationsForDestinationIdValue = firstItem;
                    }
                }

            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Destination" });
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                await PostgresService.CreatePlace(place);
                DialogService.Close(place);
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