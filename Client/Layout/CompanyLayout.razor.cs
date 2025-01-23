using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TripBUK.Client.Components;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace TripBUK.Client.Layout
{
    public partial class CompanyLayout
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
        
        private RenderFragment RenderMenuItem(MenuItem item) => builder =>
        {
            builder.OpenComponent<RadzenMenuItem>(0);
            builder.AddAttribute(1, "Text", item.Text);
            if (item.Icon != null) builder.AddAttribute(2, "Icon", item.Icon);
            if (item.Path != null) builder.AddAttribute(3, "Path", item.Path);
            if (item.Class != null) builder.AddAttribute(4, "class", item.Class);
            if (item.Items != null && item.Items.Count != 0)
            {
                builder.AddAttribute(4, "ChildContent", (RenderFragment)(childBuilder =>
                {
                    foreach (var subItem in item.Items)
                    {
                        childBuilder.AddContent(5, RenderMenuItem(subItem));
                    }
                }));
            }

            builder.CloseComponent();
        };
        
        
        private readonly List<MenuItem> _menuItems =
        [
            new()
            {
                Text = "Administration", Icon = "shield",
                Items =
                [
                    new MenuItem() { 
                        Text = "Masters", 
                        Icon = "book_2",
                        Items = [
                            new MenuItem()
                            {
                                Text = "Accounts",
                                Icon = "account_balance",
                                Path = "administration/masters/accounts",
                                Items = 
                                [
                                    new MenuItem() 
                                    { 
                                        Text = "Groups",
                                        Icon = "category",
                                        Path = "administration/masters/accounts/groups",
                                        Items = 
                                        [
                                            new MenuItem() { Text = "Create", Icon = "add", Path = "administration/masters/accounts/groups/create" },
                                        ]
                                    },
                                    new MenuItem() { Text = "Create", Icon = "add", Path = "administration/masters/accounts/create" },
                                ]
                            },
                            new MenuItem()
                            {
                                Text = "Items",
                                Icon = "inventory_2",
                                Path = "administration/masters/items",
                                Items = 
                                [
                                    new MenuItem() 
                                    { 
                                        Text = "Groups",
                                        Icon = "category",
                                        Path = "administration/masters/items/groups",
                                        Items = 
                                        [
                                            new MenuItem() { Text = "Create", Icon = "add", Path = "administration/masters/items/groups/create" },
                                        ]
                                    },
                                    new MenuItem() { Text = "Create", Icon = "add", Path = "administration/masters/items/create" },
                                ]
                            },
                            new MenuItem()
                            {
                                Text = "Units",
                                Icon = "scale",
                                Path = "administration/masters/units",
                                Items = 
                                [
                                    new MenuItem()
                                    {
                                        Text = "Conversions",
                                        Icon = "rule_settings",
                                        Path = "administration/masters/units/conversions",
                                        Items = 
                                        [
                                            new MenuItem() { Text = "Create", Icon = "add", Path = "administration/masters/units/conversions/create" },
                                        ]
                                    },
                                    new MenuItem() { Text = "Create", Icon = "add", Path = "administration/masters/units/create" },
                                ]
                            },
                            new MenuItem()
                            {
                                Text = "Bill Sundries",
                                Icon = "receipt",
                                Path = "#",
                                Items = 
                                [
                                    new MenuItem() { Text = "Create", Icon = "add", Path = "#" },
                                ]
                            },
                            new MenuItem()
                            {
                                Text = "Bill of Materials", 
                                Icon = "checklist",
                                Path = "#", 
                                Items = 
                                [
                                    new MenuItem() { Text = "Create", Icon = "add", Path = "#" },
                                ]
                            },
                            new MenuItem()
                            { 
                                Text = "Standard Narrations",
                                Icon = "description",
                                Path = "administration/masters/standard-narrations",
                                Items = 
                                [
                                    new MenuItem() { Text = "Create", Icon = "add", Path = "administration/masters/standard-narrations/create" },
                                ]
                            },
                            new MenuItem()
                            {
                                Text = "Sale Types", 
                                Icon = "sell",
                                Path = "#", 
                                Items = 
                                [
                                    new MenuItem() { Text = "Create", Icon = "add", Path = "#" },
                                ]
                            },
                            new MenuItem()
                            {
                                Text = "Purchase Types",
                                Icon = "handshake",
                                Path = "#", 
                                Items = 
                                [
                                    new MenuItem() { Text = "Create", Icon = "add", Path = "#" },
                                ]
                            },
                            new MenuItem()
                            {
                                Text = "Tax Slabs",
                                Icon = "percent",
                                Path = "#", 
                                Items = 
                                [
                                    new MenuItem() { Text = "Create", Icon = "add", Path = "#" },
                                ]
                            },
                            new MenuItem()
                            {
                                Text = "Misc. Masters", 
                                Path = "#",
                                Icon = "other_admission", 
                                Items = 
                                [
                                    new MenuItem() { Text = "Create", Icon = "add", Path = "#" },
                                ]
                            },
                            new MenuItem()
                            {
                                Text = "Bulk Modifications", 
                                Path = "#",
                                Icon = "published_with_changes", 
                                Items = 
                                [
                                    new MenuItem() { Text = "Create", Icon = "add", Path = "#" },
                                ]
                            },
                        ]
                    },
                    new MenuItem()
                    {
                        Text = "Access", 
                        Icon = "security", 
                        Items = [
                            new MenuItem()
                            {
                                Text = "Users",
                                Icon = "group",
                                Path = "administration/access/users", 
                                Items = 
                                [
                                    new MenuItem() { Text = "Create", Icon = "add", Path = "#" },
                                ]
                            },
                            new MenuItem()
                            {
                                Text = "Roles",
                                Icon = "groups",
                                Path = "administration/access/roles", 
                                Items = 
                                [
                                    new MenuItem() { Text = "Create", Icon = "add", Path = "#" },
                                ]
                            },
                            new MenuItem()
                            {
                                Text = "Permissions",
                                Icon = "shield_person",
                                Path = "#"
                            },
                        ]
                    },
                    new MenuItem()
                    {
                        Text = "Configuration",
                        Icon = "manufacturing",
                        Path = "#"
                    }
                ]
            },

            new ()
            {
                Text = "Transactions", 
                Icon = "swap_horiz",
                Items = [
                    new MenuItem() { Text = "Sales", Path = "#" },
                    new MenuItem() { Text = "Purchase", Path = "#" },
                    new MenuItem() { Text = "Sales Return (Cr. Note)", Path = "#" },
                    new MenuItem() { Text = "Purchase Return (Dr. Note)", Path = "#", Class = "rz-mb-4" },
                    new MenuItem() { Text = "Payment", Path = "#" },
                    new MenuItem() { Text = "Receipt", Path = "#" },
                    new MenuItem() { Text = "Journal", Path = "#" },
                    new MenuItem() { Text = "Contra", Path = "#" },
                    new MenuItem() { Text = "Dr. Note (w/o Items)", Path = "#" },
                    new MenuItem() { Text = "Cr. Note (w/o Items)", Path = "#", Class = "rz-mb-4" },
                    new MenuItem() { Text = "Production", Path = "#" },
                    new MenuItem() { Text = "Disassemble", Path = "#" },
                    new MenuItem() { Text = "Stock Journal", Path = "#", Class = "rz-mb-4" },
                    new MenuItem() { Text = "Material Issued to Party", Path = "#" },
                    new MenuItem() { Text = "Material Received from Party", Path = "#", Class = "rz-mb-4" },
                    new MenuItem() { Text = "Physical Stock", Path = "#", Class = "rz-mb-4" },
                    new MenuItem() { Text = "GST Misc. Utilities", Path = "#" },
                ]
            },
            new ()
            {
                Text = "Reports", 
                Icon = "print",
                Items = [
                    new MenuItem()
                    {
                        Text = "Final Results",
                        Items = [
                            new MenuItem(){ Text = "Balance Sheet", Path = "#" },
                            new MenuItem(){ Text = "Profit & Loss", Path = "#" },
                            new MenuItem(){ Text = "Payment & Receipt", Path = "#" },
                            new MenuItem(){ Text = "Income & Expense", Path = "#" },
                        ]
                    },
                    new MenuItem()
                    {
                        Text = "Trial Balance",
                        Items = [
                            new MenuItem(){ Text = "Closing", Path = "#" },
                            new MenuItem(){ Text = "Opening", Path = "#" },
                        ]
                    },
                    new MenuItem()
                    {
                        Text = "Account Books",
                        Items = [
                            new MenuItem(){ Text = "Day Book", Path = "#" },
                            new MenuItem(){ Text = "Account Ledger", Path = "#" },
                            new MenuItem(){ Text = "Cash/Bank Book", Path = "#" },
                            new MenuItem()
                            {
                                Text = "Account Registers",
                                Items = [
                                    new MenuItem(){ Text = "Sales Register", Path = "#" },
                                    new MenuItem(){ Text = "Purchase Register", Path = "#" },
                                    new MenuItem(){ Text = "Sale Return Register", Path = "#" },
                                    new MenuItem(){ Text = "Purchase Return Register", Path = "#" },
                                    new MenuItem(){ Text = "Payment Register", Path = "#" },
                                    new MenuItem(){ Text = "Receipt Register", Path = "#" },
                                    new MenuItem(){ Text = "Journal Register", Path = "#" },
                                    new MenuItem(){ Text = "Contra Register", Path = "#" },
                                    new MenuItem(){ Text = "Debit Note Register", Path = "#" },
                                    new MenuItem(){ Text = "Credit Note Register", Path = "#" },
                                ]
                            },
                            new MenuItem(){ Text = "Account Activity", Path = "#" },
                            new MenuItem(){ Text = "Party Day Book", Path = "#" },
                        ]
                    },
                    new MenuItem()
                    {
                        Text = "Account Summaries",
                        Items = [
                            new MenuItem(){ Text = "Daily Balances", Path = "#" },
                            new MenuItem(){ Text = "Daily Summary", Path = "#" },
                            new MenuItem(){ Text = "Monthly Summary", Path = "#" },
                            new MenuItem(){ Text = "Consolidated Summary", Path = "#" },
                            new MenuItem(){ Text = "Transaction Summary", Path = "#" },
                            new MenuItem(){ Text = "Min/Max Cash Balances", Path = "#" },
                            new MenuItem(){ Text = "Account Ledger Comparison", Path = "#" },
                            new MenuItem(){ Text = "Settlement Summary", Path = "#" },
                            new MenuItem(){ Text = "Settlement Details", Path = "#" },
                            new MenuItem(){ Text = "Ledger Abstract", Path = "#" },
                            new MenuItem(){ Text = "Unmoved Accounts", Path = "#" },
                            new MenuItem(){ Text = "UnBilled Parties", Path = "#", Class = "rz-mb-4" },
                            new MenuItem(){ Text = "Parties with Invalid PAN", Path = "#" },
                            new MenuItem(){ Text = "Party Turnover Details", Path = "#" },
                            new MenuItem(){ Text = "Cash/Credit Sales Summary", Path = "#" },
                            new MenuItem(){ Text = "Cash/Credit Purchase Summary", Path = "#" },
                            new MenuItem(){ Text = "Party Cash Summary", Path = "#" },
                            new MenuItem(){ Text = "Cash Against Invoices", Path = "#" },
                            
                        ]
                    },
                    new MenuItem()
                    {
                        Text = "Outstanding Analysis",
                        Items = [
                            new MenuItem(){ Text = "Amount Receivable", Path = "#" },
                            new MenuItem(){ Text = "Bills Receivable", Path = "#" },
                            new MenuItem(){ Text = "Ageing Receivable", Path = "#" },
                            new MenuItem(){ Text = "Ageing Receivable (FIFO)", Path = "#", Class = "rz-mb-4" },
                            new MenuItem(){ Text = "Amount Payable", Path = "#" },
                            new MenuItem(){ Text = "Bills Payable", Path = "#" },
                            new MenuItem(){ Text = "Ageing Payable", Path = "#" },
                            new MenuItem(){ Text = "Ageing Payable (FIFO)", Path = "#", Class = "rz-mb-4" },
                            new MenuItem(){ Text = "Bills Summary", Path = "#" },
                            new MenuItem(){ Text = "Bills-wise Statement", Path = "#" },
                            new MenuItem(){ Text = "Bill Reference Details", Path = "#" },
                            new MenuItem(){ Text = "On Account Entries", Path = "#" },
                            new MenuItem(){ Text = "Statement of A/c", Path = "#" },
                            new MenuItem(){ Text = "Confirmation of A/c", Path = "#" },
                            new MenuItem(){ Text = "Payment Reminder", Path = "#" },
                            
                        ]
                    },
                ]
            },
            new () { Text = "Help", Icon = "help", Path = "#" }

        ];
    }
}