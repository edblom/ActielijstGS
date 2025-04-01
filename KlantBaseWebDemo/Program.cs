using Radzen;
using KlantBaseWebDemo.Components;
using Microsoft.EntityFrameworkCore;
using KlantBaseWebDemo.Data;
using Microsoft.AspNetCore.Identity;
using KlantBaseWebDemo.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddHubOptions(options => options.MaximumReceiveMessageSize = 10 * 1024 * 1024);

builder.Services.AddControllers();
builder.Services.AddRadzenComponents();
builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = "KlantBaseWebDemoTheme";
    options.Duration = TimeSpan.FromDays(365);
});

// Configureer HttpClient voor KlantBase API zonder authenticatie
builder.Services.AddHttpClient("KlantBaseApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["KlantBaseApiUrl"] ?? "https://jouw-api-url/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Bestaande services
builder.Services.AddScoped<KlantBaseWebDemo.KlantBaseService>();
builder.Services.AddDbContext<KlantBaseWebDemo.Data.KlantBaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("KlantBaseConnection"));
});
builder.Services.AddHttpClient("KlantBaseWebDemo")
    .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler { UseCookies = false })
    .AddHeaderPropagation(o => o.Headers.Add("Cookie"));
builder.Services.AddHeaderPropagation(o => o.Headers.Add("Cookie"));

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddScoped<KlantBaseWebDemo.SecurityService>();
builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("KlantBaseConnection"));
});
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers().AddOData(o =>
{
    var oDataBuilder = new ODataConventionModelBuilder();
    oDataBuilder.EntitySet<ApplicationUser>("ApplicationUsers");
    var usersType = oDataBuilder.StructuralTypes.First(x => x.ClrType == typeof(ApplicationUser));
    usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.Password)));
    usersType.AddProperty(typeof(ApplicationUser).GetProperty(nameof(ApplicationUser.ConfirmPassword)));
    oDataBuilder.EntitySet<ApplicationRole>("ApplicationRoles");
    o.AddRouteComponents("odata/Identity", oDataBuilder.GetEdmModel())
        .Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});

builder.Services.AddScoped<AuthenticationStateProvider, KlantBaseWebDemo.ApplicationAuthenticationStateProvider>();
builder.Services.AddLocalization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseHeaderPropagation();
app.UseRequestLocalization(options => options.AddSupportedCultures("nl-NL", "en", "da-DK")
    .AddSupportedUICultures("nl-NL", "en", "da-DK")
    .SetDefaultCulture("nl-NL"));
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Services.CreateScope().ServiceProvider.GetRequiredService<ApplicationIdentityDbContext>().Database.Migrate();

app.Run();