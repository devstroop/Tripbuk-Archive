using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace ERP.Client.Pages.Admin.Masters.Accounts
{
    public partial class Accounts
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

        private IEnumerable<ERP.Server.Models.Postgres.Account> _accounts;

        private RadzenDataGrid<ERP.Server.Models.Postgres.Account> _grid0;
        private int _count;

        private string search = "";

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
                var result = await PostgresService.GetAccounts(filter: $@"(contains(AccountName,""{search}"") or contains(Alias,""{search}"") or contains(PrintName,""{search}"") or contains(Address,""{search}"") or contains(City,""{search}"") or contains(State,""{search}"") or contains(PinCode,""{search}"") or contains(Country,""{search}"") or contains(Phone,""{search}"") or contains(Mobile,""{search}"") or contains(Whatsapp,""{search}"") or contains(Email,""{search}"") or contains(ContactPerson,""{search}"") or contains(Gstin,""{search}"") or contains(Pan,""{search}"") or contains(OpeningBalanceType,""{search}"")) and {(string.IsNullOrEmpty(args.Filter)? "true" : args.Filter)}", expand: "AccountGroup", orderby: $"{args.OrderBy}", top: args.Top, skip: args.Skip, count:args.Top != null && args.Skip != null);
                _accounts = result.Value.AsODataEnumerable();
                _count = result.Count;
            }
            catch (System.Exception ex)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error, Summary = $"Error", Detail = $"Unable to load Accounts" });
            }
        }

        private async Task ShowAccountDetails(ERP.Server.Models.Postgres.Account args)
        {
            await DialogService.OpenAsync<AccountDetails>("Account Details", new Dictionary<string, object> { {"Id", args.Id} }, new DialogOptions()
            {
                Width = "800px",
            });
        }

        private async Task GridDeleteButtonClick(MouseEventArgs args, ERP.Server.Models.Postgres.Account account)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await PostgresService.DeleteAccount(id:account.Id);

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
                    Detail = $"Unable to delete Account"
                });
            }
        }
    }
}