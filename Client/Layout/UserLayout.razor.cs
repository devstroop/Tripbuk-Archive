using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace ERP.Client.Layout
{
    public partial class UserLayout
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
        protected SecurityService Security { get; set; }

        private bool _sidebarExpanded { get; set; } = false;

        protected override void OnInitialized()
        {
            _sidebarExpanded = false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _sidebarExpanded = false;
                StateHasChanged();
            }
        }

        private void SidebarToggleClick()
        {
            _sidebarExpanded = !_sidebarExpanded;
        }

        private void ProfileMenuClick(RadzenProfileMenuItem args)
        {
            if (args.Value == "Logout")
            {
                Security.Logout();
            }
        }
        
        class MenuItem
        {
            public required string Text { get; set; }
            public string Icon { get; set; }
            public string Path { get; set; }
            public string Class { get; set; }
            public List<MenuItem> Items { get; set; }
        }
        
        private readonly List<MenuItem> _menuItems =
        [
            new() { Text = "Dashboard", Icon = "dashboard", Path = "dashboard" },
            new()
            {
                Text = "Management", Icon = "hub",
                Items =
                [
                    new MenuItem() { 
                        Text = "Masters", 
                        Icon = "book_2",
                        Items = [
                            new MenuItem() { Text = "Accounts", Path = "management/masters/accounts" },
                            new MenuItem() { Text = "Account Groups", Path = "management/masters/account-groups" },
                            new MenuItem() { Text = "Standard Narrations", Path = "management/masters/standard-narrations", Class = "rz-mb-4" },
                            new MenuItem() { Text = "Items", Path = "management/masters/items" },
                            new MenuItem() { Text = "Item Groups", Path = "management/masters/item-groups", Class = "rz-mb-4" },
                            new MenuItem() { Text = "Units", Path = "management/masters/units" },
                            new MenuItem() { Text = "Unit Conversions", Path = "management/masters/unit-conversions" },
                            new MenuItem() { Text = "Bill Sundries", Path = "#" },
                            new MenuItem() { Text = "Bill of Materials", Path = "#", Class = "rz-mb-4" },
                            new MenuItem() { Text = "Sale Types", Path = "#" },
                            new MenuItem() { Text = "Purchase Types", Path = "#" },
                            new MenuItem() { Text = "Tax Slabs", Path = "#", Class = "rz-mb-4" },
                            new MenuItem() { Text = "Misc. Masters", Path = "#", Class = "rz-mb-4" },
                            new MenuItem() { Text = "Bulk Modifications", Path = "#", Class = "rz-mb-4" },
                        ]
                    },
                    new MenuItem() { Text = "Access", Icon = "shield_person", Path = "#" },
                    new MenuItem() { Text = "Configuration", Icon = "manufacturing", Path = "#" }
                ]
            },

            new () { Text = "Transactions", Icon = "swap_horiz", Path = "#" },
            new () { Text = "Reports", Icon = "analytics", Path = "#" },
            new () { Text = "Help", Icon = "help", Path = "#" }

        ];
    }
}