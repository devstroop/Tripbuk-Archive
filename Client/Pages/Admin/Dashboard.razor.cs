using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;

namespace Tripbuk.Client.Pages.Admin;

public partial class Dashboard
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
    protected SecurityService Security { get; set; }

}