using Microsoft.AspNetCore.Components;

namespace Tripbuk.Client.Components;

public partial class ImportWizard
{
    [Parameter]
    public EventCallback OnClose { get; set; }

    private void Close()
    {
        OnClose.InvokeAsync();
    }
}