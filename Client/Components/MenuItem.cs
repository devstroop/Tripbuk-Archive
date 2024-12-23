namespace ERP.Client.Components;

public class MenuItem
{
    public required string Text { get; set; }
    public string Icon { get; set; }
    public string Path { get; set; }
    public string Class { get; set; }
    public List<MenuItem> Items { get; set; }
}