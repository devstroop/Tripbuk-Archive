using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using Microsoft.JSInterop;
using System.Globalization;
using Microsoft.AspNetCore.Components.Authorization;
using Tripbuk.Client.Components;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddRadzenComponents();
builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = "TripbukTheme";
    options.Duration = TimeSpan.FromDays(30);
});
builder.Services.AddScoped<ProgressDialogState>();
builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddLocalization();
builder.Services.AddScoped<Tripbuk.Client.PostgresService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddHttpClient("Tripbuk.Server", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Tripbuk.Server"));
builder.Services.AddScoped<Tripbuk.Client.SecurityService>();
builder.Services.AddScoped<AuthenticationStateProvider, Tripbuk.Client.ApplicationAuthenticationStateProvider>();
// builder.Services.AddHttpClient("Viator", client =>
// {
//     client.BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}/proxy/viator/");
// });
builder.Services.AddScoped<Tripbuk.Client.Services.ViatorService>();
var host = builder.Build();
var jsRuntime = host.Services.GetRequiredService<Microsoft.JSInterop.IJSRuntime>();
var culture = await jsRuntime.InvokeAsync<string>("Radzen.getCulture");
if (!string.IsNullOrEmpty(culture))
{
    CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
    CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(culture);
}
await host.RunAsync();