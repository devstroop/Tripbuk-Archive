namespace Tripbuk.Client.Components;

public class ProgressDialogState
{
    public string Message { get; private set; }
    public string Title { get; private set; }
    public double Progress { get; private set; }
    
    public event Action OnChange;

    public void SetMessage(string message)
    {
        Message = message;
        NotifyStateChanged();
    }

    public void SetTitle(string title)
    {
        Title = title;
        NotifyStateChanged();
    }

    public void SetProgress(double progress)
    {
        Progress = progress;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}