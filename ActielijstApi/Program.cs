using ActielijstApi;
using ActielijstApi.Data;
using ActielijstApi.Models;
using ActielijstApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// Voeg DbContext toe
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging()
           .EnableDetailedErrors()
           .LogTo(log => Debug.WriteLine(log), LogLevel.Information));

// Voeg SmtpSettings toe
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

// Voeg controllers toe
builder.Services.AddControllers();

// Voeg Swagger toe
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "KlantBase API", Version = "v1" });

    // Voeg XML-documentatie toe
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
    else
    {
        Console.WriteLine($"Waarschuwing: XML-documentatiebestand niet gevonden op {xmlPath}");
    }
});

// Voeg services toe
builder.Services.AddScoped<CorrespondenceService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<GlobalsService>();
builder.Services.AddScoped<IActieService, ActieService>();

// Voeg CORS toe
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorFrontend", policy =>
    {
        policy.WithOrigins("https://localhost:7002") // Pas aan naar de frontend-poort
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // Als authenticatie nodig is
    });
});

// Voeg logging toe
builder.Services.AddLogging(logging =>
{
    logging.ClearProviders();
    logging.AddConsole();
    logging.AddDebug();
});

var app = builder.Build();
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogDebug("Applicatie gestart in Debug-modus op {Time}", DateTime.Now);
logger.LogInformation("Dit is een informatiebericht bij het starten.");

// Configureer de HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "KlantBase API V1");
        c.RoutePrefix = string.Empty;
    });

    // Redirect root naar Swagger UI
    app.Use(async (context, next) =>
    {
        if (context.Request.Path == "/")
        {
            context.Response.Redirect("/swagger/index.html");
            return;
        }
        await next();
    });
}
else
{
    app.UseExceptionMiddleware();
}

// CORS-middleware moet vóór UseAuthorization en UseEndpoints
app.UseCors("AllowBlazorFrontend");

app.UseHttpsRedirection();
app.UseAuthorization();

// Registreer minimal API-endpoints uit Endpoints.cs
app.RegisterEndpoints();

// Registreer controller-endpoints
app.MapControllers();

app.Run();