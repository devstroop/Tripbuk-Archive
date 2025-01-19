using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace ERP.Client.Pages.Admin.SmtpConfigs
{
    public partial class AddSmtpConfig
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

        private bool Ssl
        {
            get => smtpConfig.Ssl == true;
            set => smtpConfig.Ssl = value;
        }

        protected override async Task OnInitializedAsync()
        {
            smtpConfig = new ERP.Server.Models.Postgres.SmtpConfig();
        }
        protected bool errorVisible;
        protected ERP.Server.Models.Postgres.SmtpConfig smtpConfig;

        protected async Task FormSubmit()
        {
            try
            {
                var result = await PostgresService.CreateSmtpConfig(smtpConfig);
                DialogService.Close(smtpConfig);
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


        protected bool hasChanges = false;
        protected bool canEdit = true;

        [Inject]
        protected SecurityService Security { get; set; }
    }
}