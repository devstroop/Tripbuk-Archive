using System.Globalization;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace TripBUK.Client.Components
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
    }
}