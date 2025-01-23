using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using TripBUK.Client;
using Microsoft.JSInterop;
using System.Globalization;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddRadzenComponents();
builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = "TripBUKTheme";
    options.Duration = TimeSpan.FromDays(365);
});
builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddLocalization();
builder.Services.AddScoped<TripBUK.Client.PostgresService>();
builder.Services.AddAuthorizationCore();
builder.Services.AddHttpClient("TripBUK.Server", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
builder.Services.AddTransient(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("TripBUK.Server"));
builder.Services.AddScoped<TripBUK.Client.SecurityService>();
builder.Services.AddScoped<AuthenticationStateProvider, TripBUK.Client.ApplicationAuthenticationStateProvider>();
var host = builder.Build();
var jsRuntime = host.Services.GetRequiredService<Microsoft.JSInterop.IJSRuntime>();
var culture = await jsRuntime.InvokeAsync<string>("Radzen.getCulture");
if (!string.IsNullOrEmpty(culture))
{
    CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(culture);
    CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo(culture);
}

await host.RunAsync();