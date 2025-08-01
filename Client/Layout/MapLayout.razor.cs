using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Tripbuk.Client.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace Tripbuk.Client.Layout
{
    public partial class MapLayout
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

        private bool SidebarExpanded { get; set; } = false;

        protected override void OnInitialized()
        {
            SidebarExpanded = false;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                SidebarExpanded = false;
                StateHasChanged();
            }
        }

        private void SidebarToggleClick()
        {
            SidebarExpanded = !SidebarExpanded;
        }

        private void ProfileMenuClick(RadzenProfileMenuItem args)
        {
            if (args.Value == "Logout")
            {
                Security.Logout();
            }
        }
        

        private string Search { get; set; }
        
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
            new ()
            {
                Text = "Discover", 
                Icon = "explore",
                Items = []
            },
            new ()
            {
                Text = "Buckets",
                Icon = "checklist", 
                // Path = "#"
            },
            new ()
            {
                Text = "Trips",
                Icon = "your_trips", 
                // Path = "#"
            },
            // new ()
            // {
            //     Text = "Bookings",
            //     Icon = "your_trips", 
            //     // Path = "#"
            // }

        ];
    }
}