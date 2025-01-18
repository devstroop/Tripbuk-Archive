using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace ERP.Client.Pages.Admin.Masters.Units
{
    public partial class EditUnit
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

        [Parameter]
        public int Id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Unit = await PostgresService.GetUnitById(id:Id);
        }
        protected bool ErrorVisible;
        protected ERP.Server.Models.Postgres.Unit Unit;

        protected async Task FormSubmit()
        {
            try
            {
                var result = await PostgresService.UpdateUnit(id:Id, Unit);
                if (result.StatusCode == System.Net.HttpStatusCode.PreconditionFailed)
                {
                     HasChanges = true;
                     CanEdit = false;
                     return;
                }
                DialogService.Close(Unit);
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


        protected async Task ReloadButtonClick(MouseEventArgs args)
        {
            HasChanges = false;
            CanEdit = true;

            Unit = await PostgresService.GetUnitById(id:Id);
        }
    }
}