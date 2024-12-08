using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace ERP.Client.Pages.Administration.Accounts
{
    public partial class AccountMasters
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

        protected IEnumerable<ERP.Server.Models.Postgres.AccountMaster> accountMasters;

        protected RadzenDataGrid<ERP.Server.Models.Postgres.AccountMaster> grid0;
        protected int count;

        protected string search = "";

        [Inject]
        protected SecurityService Security { get; set; }

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
                var result = await PostgresService.GetAccountMasters(filter: $@"(contains(AccountName,""{search}"") or contains(Alias,""{search}"") or contains(PrintName,""{search}"") or contains(Address,""{search}"") or contains(City,""{search}"") or contains(State,""{search}"") or contains(PinCode,""{search}"") or contains(Country,""{search}"") or contains(Phone,""{search}"") or contains(Mobile,""{search}"") or contains(Whatsapp,""{search}"") or contains(Email,""{search}"") or contains(ContactPerson,""{search}"") or contains(Gstin,""{search}"") or contains(Pan,""{search}"") or contains(OpeningBalanceType,""{search}"") or contains(CreatedBy,""{search}"") or contains(UpdatedBy,""{search}"") or contains(DeletedBy,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", expand: "Master,AccountGroupMaster", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                accountMasters = result.Value.AsODataEnumerable();
                count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load AccountMasters" });
            }
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddAccountMaster>("Add AccountMaster", null);
            await grid0.Reload();
        }

        protected async Task EditRow(ERP.Server.Models.Postgres.AccountMaster args)
        {
            await DialogService.OpenAsync<EditAccountMaster>("Edit AccountMaster", new Dictionary<string, object> { {"MasterId", args.MasterId} });
            await grid0.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, ERP.Server.Models.Postgres.AccountMaster accountMaster)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await PostgresService.DeleteAccountMaster(masterId:accountMaster.MasterId);

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
                    Detail = $"Unable to delete AccountMaster"
                });
            }
        }
    }
}