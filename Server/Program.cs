using Radzen;
using Tripbuk.Server.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.OData;
using Tripbuk.Server.Data;
using Microsoft.AspNetCore.Identity;
using Tripbuk.Server.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Tripbuk.Client.Components;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddRazorComponents().AddInteractiveServerComponents().AddHubOptions(options => options.MaximumReceiveMessageSize = 10 * 1024 * 1024).AddInteractiveWebAssemblyComponents();
builder.Services.AddControllers();
builder.Services.AddRadzenComponents();
builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = "TripbukTheme";
    options.Duration = TimeSpan.FromDays(30);
});
builder.Services.AddScoped<ProgressDialogState>();
builder.Services.AddHttpClient();
builder.Services.AddLocalization();
builder.Services.AddScoped<Tripbuk.Server.PostgresService>();
builder.Services.AddDbContext<Tripbuk.Server.Data.PostgresContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
});
builder.Services.AddControllers().AddOData(opt =>
{
    var oDataBuilderPostgres = new ODataConventionModelBuilder();
    oDataBuilderPostgres.EntitySet<Tripbuk.Server.Models.Postgres.Destination>("Destinations");
    oDataBuilderPostgres.EntitySet<Tripbuk.Server.Models.Postgres.LocationCenter>("LocationCenters");
    oDataBuilderPostgres.EntitySet<Tripbuk.Server.Models.Postgres.Place>("Places");
    oDataBuilderPostgres.EntitySet<Tripbuk.Server.Models.Postgres.Tag>("Tags");
    oDataBuilderPostgres.Function("ParentTags").Returns<Tripbuk.Server.Models.Postgres.ParentTag>();
    oDataBuilderPostgres.Function("PlaceTags").Returns<Tripbuk.Server.Models.Postgres.PlaceTag>();
    opt.AddRouteComponents("odata/Postgres", oDataBuilderPostgres.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});
builder.Services.AddScoped<Tripbuk.Client.PostgresService>();
builder.Services.AddHttpClient("Tripbuk.Server").ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { UseCookies = false }).AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddScoped<Tripbuk.Client.SecurityService>();
builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
});
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<ApplicationIdentityDbContext>().AddDefaultTokenProviders();
builder.Services.AddTransient<IUserStore<ApplicationUser>, MultiTenancyUserStore>();
builder.Services.AddControllers().AddOData(o =>
{
    var oDataBuilder = new ODataConventionModelBuilder();
    oDataBuilder.EntitySet<ApplicationUser>("ApplicationUsers");
    var usersType = oDataBuilder.StructuralTypes.First(x => x.ClrType == typeof(ApplicationUser));
    usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.Password)));
    usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.ConfirmPassword)));
    oDataBuilder.EntitySet<ApplicationRole>("ApplicationRoles");
    oDataBuilder.EntitySet<ApplicationTenant>("ApplicationTenants");
    o.AddRouteComponents("odata/Identity", oDataBuilder.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});
builder.Services.AddScoped<AuthenticationStateProvider, Tripbuk.Client.ApplicationAuthenticationStateProvider>();
builder.Services.AddHttpClient("Tripbuk.Server").AddHeaderPropagation(o => o.Headers.Add("Cookie"));
// builder.Services.AddHttpClient("Viator", client =>
// {
//     client.BaseAddress = new Uri("https://api.sandbox.viator.com/partner/");
// });
builder.Services.AddScoped<Tripbuk.Client.Services.ViatorService>();
builder.Services.AddScoped<Tripbuk.Client.Services.UnsplashService>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
// app.UseCors("_AllowedOrigins");
app.MapControllers();
app.UseRequestLocalization(options => options.AddSupportedCultures("en", "hi").AddSupportedUICultures("en", "hi").SetDefaultCulture("en"));
app.UseHeaderPropagation();
app.UseSession();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapReverseProxy();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode().AddInteractiveWebAssemblyRenderMode().AddAdditionalAssemblies(typeof(Tripbuk.Client._Imports).Assembly);
app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>().Database.Migrate();
app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>().SeedTenantsAdmin().Wait();
app.Services.CreateScope().ServiceProvider.GetRequiredService<PostgresContext>().Database.Migrate();
app.Run();