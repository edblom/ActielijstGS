using KlantBaseWASM;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using KlantBaseWASM.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Register Radzen services
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddRadzenComponents();

// Configure HttpClient for ActielijstAPI (local during development)
builder.Services.AddHttpClient("ActielijstAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:44361"); // Update to your local API URL
});

// Register services with dependency injection
builder.Services.AddScoped<ActieService>();
builder.Services.AddScoped<CorrespondenceService>();
builder.Services.AddScoped<WerknemerService>();

await builder.Build().RunAsync();