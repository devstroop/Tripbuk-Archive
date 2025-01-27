using System.Globalization;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace Tripbuk.Client.Components
{
    public partial class CulturePicker
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

        private readonly object[] _cultures = new[]
            { new { Text = "English", Value = "en" }, new { Text = "हिन्दी", Value = "hi" } };
        
        private string _culture;

        protected override void OnInitialized()
        {
            _culture = CultureInfo.CurrentCulture.Name;
        }

        private void ChangeCulture()
        {
            var redirect = new Uri(NavigationManager.Uri).GetComponents(UriComponents.PathAndQuery | UriComponents.Fragment, UriFormat.UriEscaped);

            var query = $"?culture={Uri.EscapeDataString(_culture)}&redirectUri={redirect}";

            NavigationManager.NavigateTo("Culture/SetCulture" + query, forceLoad: true);
        }
        
        
        void ShowCultureOptions(MouseEventArgs args)
        {
            ContextMenuService.Open(args,
                new List<ContextMenuItem> {
                    new ContextMenuItem(){ Text = "English", Value = "en" },
                    new ContextMenuItem(){ Text = "हिन्दी", Value = "hi" },
                }, OnCultureItemClick);
        }

        void OnCultureItemClick(MenuItemEventArgs args)
        {
            // console.Log($"Menu item with Value={args.Value} clicked");
            if(!args.Value.Equals(3) && !args.Value.Equals(4))
            {
                _culture = args.Value.ToString();
                ChangeCulture();
                ContextMenuService.Close();
            }
        }
    }
}