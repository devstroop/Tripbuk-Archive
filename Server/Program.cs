using Radzen;
using TripBUK.Server.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.OData;
using TripBUK.Server.Data;
using Microsoft.AspNetCore.Identity;
using TripBUK.Server.Models;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents().AddHubOptions(options => options.MaximumReceiveMessageSize = 10 * 1024 * 1024).AddInteractiveWebAssemblyComponents();
builder.Services.AddControllers();
builder.Services.AddRadzenComponents();
builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = "TripBUKTheme";
    options.Duration = TimeSpan.FromDays(365);
});
builder.Services.AddHttpClient();
builder.Services.AddLocalization();
builder.Services.AddScoped<TripBUK.Server.PostgresService>();
builder.Services.AddDbContext<TripBUK.Server.Data.PostgresContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection"));
});
builder.Services.AddControllers().AddOData(opt =>
{
    var oDataBuilderPostgres = new ODataConventionModelBuilder();
    oDataBuilderPostgres.EntitySet<TripBUK.Server.Models.Postgres.AccountGroup>("AccountGroups");
    oDataBuilderPostgres.EntitySet<TripBUK.Server.Models.Postgres.Account>("Accounts");
    oDataBuilderPostgres.EntitySet<TripBUK.Server.Models.Postgres.ItemGroup>("ItemGroups");
    oDataBuilderPostgres.EntitySet<TripBUK.Server.Models.Postgres.Item>("Items");
    oDataBuilderPostgres.EntitySet<TripBUK.Server.Models.Postgres.StandardNarration>("StandardNarrations");
    oDataBuilderPostgres.EntitySet<TripBUK.Server.Models.Postgres.UnitConversion>("UnitConversions");
    oDataBuilderPostgres.EntitySet<TripBUK.Server.Models.Postgres.Unit>("Units");
    oDataBuilderPostgres.EntitySet<TripBUK.Server.Models.Postgres.SmtpConfig>("SmtpConfigs");
    opt.AddRouteComponents("odata/Postgres", oDataBuilderPostgres.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});
builder.Services.AddScoped<TripBUK.Client.PostgresService>();
builder.Services.AddHttpClient("TripBUK.Server").ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { UseCookies = false }).AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddScoped<TripBUK.Client.SecurityService>();
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
builder.Services.AddScoped<AuthenticationStateProvider, TripBUK.Client.ApplicationAuthenticationStateProvider>();
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
app.MapControllers();
app.UseRequestLocalization(options => options.AddSupportedCultures("en", "hi").AddSupportedUICultures("en", "hi").SetDefaultCulture("en"));
app.UseHeaderPropagation();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode().AddInteractiveWebAssemblyRenderMode().AddAdditionalAssemblies(typeof(TripBUK.Client._Imports).Assembly);
app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>().Database.Migrate();
app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>().SeedTenantsAdmin().Wait();
app.Run();