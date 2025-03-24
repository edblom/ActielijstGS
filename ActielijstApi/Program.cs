using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ActielijstApi.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.Json;
using System.Linq;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Memo API", Version = "v1" });
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});
builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Memo API v1"));
}
else
{
    app.UseExceptionMiddleware();
}

app.UseCors("AllowAll");

// Login Endpoint
app.MapPost("/api/login", async (LoginRequest login, ApplicationDbContext context) =>
{
    var werknemer = await context.Werknemers
        .FirstOrDefaultAsync(w => w.Voornaam == login.Voornaam && w.FldLoginNaam == login.FldLoginNaam);

    if (werknemer == null)
    {
        return Results.Unauthorized();
    }

    return Results.Ok(new
    {
        WerknId = werknemer.WerknId,
        Voornaam = werknemer.Voornaam,
        Initialen = werknemer.Initialen
    });
})
.WithName("Login")
.WithOpenApi();

// Werknemers Endpoint
app.MapGet("/api/werknemers", async (ApplicationDbContext context) =>
{
    var workers = await context.Werknemers.ToListAsync();
    return Results.Ok(workers);
})
.WithName("GetWerknemers")
.WithOpenApi();

// Memo Endpoints
app.MapGet("/api/memos/user/{userId:int}/{filterType}", async (int userId, string filterType, ApplicationDbContext context) =>
{
    var memos = filterType.ToLower() switch
    {
        "assigned" => await context.Memos.Where(m => (m.FldMActieVoor.HasValue && m.FldMActieVoor.Value == userId) || (m.FldMActieVoor2.HasValue && m.FldMActieVoor2.Value == userId)).ToListAsync(),
        "created" => await context.Memos.Where(m => m.WerknId.HasValue && m.WerknId.Value == userId).ToListAsync(),
        _ => await context.Memos.Where(m => (m.FldMActieVoor.HasValue && m.FldMActieVoor.Value == userId) || (m.FldMActieVoor2.HasValue && m.FldMActieVoor2.Value == userId) || (m.WerknId.HasValue && m.WerknId.Value == userId)).ToListAsync()
    };
    return Results.Ok(memos);
})
.WithName("GetMemosForUser")
.WithOpenApi();

app.MapGet("/api/actiesoorten/all", async (ApplicationDbContext context) =>
{
    try
    {
        var allActiesoorten = await context.ActieSoorten.ToListAsync();
        return Results.Ok(allActiesoorten);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.WithName("GetAllActiesoorten")
.WithOpenApi();

app.MapGet("/api/memos/all", async (ApplicationDbContext context) =>
{
    try
    {
        var allMemos = await context.Memos.ToListAsync();
        return Results.Ok(allMemos);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.WithName("GetAllMemos")
.WithOpenApi();

app.MapPost("/api/memos", async (Memo memo, ApplicationDbContext context) =>
{
    context.Memos.Add(memo);
    await context.SaveChangesAsync();
    return Results.Created($"/api/memos/user/{memo.FldMActieVoor ?? 0}/assigned", memo);
})
.WithName("PostMemo")
.WithOpenApi();

app.MapPut("/api/memos/{id}", async (int id, Memo updatedMemo, ApplicationDbContext context) =>
{
    var existingMemo = await context.Memos.FindAsync(id);
    if (existingMemo == null) return Results.NotFound();

    if (id != updatedMemo.FldMid) return Results.BadRequest();

    existingMemo.FldOmschrijving = updatedMemo.FldOmschrijving;
    existingMemo.FldMActieVoor = updatedMemo.FldMActieVoor;
    existingMemo.FldMActieVoor2 = updatedMemo.FldMActieVoor2;
    existingMemo.FldMActieDatum = updatedMemo.FldMActieDatum;
    existingMemo.FldMActieSoort = updatedMemo.FldMActieSoort;
    existingMemo.WerknId = updatedMemo.WerknId;
    existingMemo.FldMPrioriteit = updatedMemo.FldMPrioriteit;

    try
    {
        await context.SaveChangesAsync();
        return Results.Ok(existingMemo);
    }
    catch (DbUpdateConcurrencyException ex)
    {
        return Results.Conflict(new { error = "Concurrency conflict: The record was modified by another user." + ex.Message });
    }
})
.WithName("PutMemo")
.WithOpenApi();

app.MapDelete("/api/memos/{id}", async (int id, ApplicationDbContext context) =>
{
    var memo = await context.Memos.FindAsync(id);
    if (memo == null) return Results.NotFound();
    context.Memos.Remove(memo);
    await context.SaveChangesAsync();
    return Results.NoContent();
})
.WithName("DeleteMemo")
.WithOpenApi();

app.MapPatch("/api/memos/{id}", async (int id, PatchMemoDto data, ApplicationDbContext context) =>
{
    var memo = await context.Memos.FindAsync(id);
    if (memo == null) return Results.NotFound();

    if (data.fldMActieGereed.HasValue)
    {
        memo.fldMActieGereed = data.fldMActieGereed;
        await context.SaveChangesAsync();
    }
    return Results.Ok(memo);
})
.WithName("PatchMemoStatus")
.WithOpenApi();

app.MapGet("/api/memos/test-error", () =>
{
    throw new ArgumentException("Dit is een testfout!");
})
.WithName("TestError")
.WithOpenApi();

// Prioriteiten Endpoint
app.MapGet("/api/priorities", async (ApplicationDbContext context) =>
{
    try
    {
        var priorities = await context.StblPriorities.ToListAsync();
        return Results.Ok(priorities);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.WithName("GetPriorities")
.WithOpenApi();

// Inspecties Endpoint
app.MapGet("/api/inspecties", async (ApplicationDbContext context) =>
{
    try
    {
        var inspecties = await context.Inspecties.ToListAsync();
        Console.WriteLine(JsonSerializer.Serialize(inspecties));
        return Results.Ok(inspecties);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
})
.WithName("GetInspecties")
.WithOpenApi();

// Upcoming Inspections Endpoint
app.MapGet("/api/upcominginspections", async (ApplicationDbContext context, string inspecteurId, bool? includeMetadata) =>
{
    try
    {
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        if (string.IsNullOrEmpty(inspecteurId))
        {
            return Results.BadRequest("InspecteurId is verplicht.");
        }

        var queryStart = stopwatch.ElapsedMilliseconds;
        var inspecties = await context.AankomendeInspecties
            .Where(i =>
                (i.InspecteurId == inspecteurId || (i.ExtraMedewerker != null && i.ExtraMedewerker == inspecteurId))
                && i.Toegewezen == true)
            .OrderBy(i => i.DatumGereed)
            .ToListAsync();
        var queryEnd = stopwatch.ElapsedMilliseconds;
        Console.WriteLine($"Databasequery duurde: {queryEnd - queryStart} ms");

        var debugStart = stopwatch.ElapsedMilliseconds;
        Console.WriteLine(JsonSerializer.Serialize(inspecties));
        var debugEnd = stopwatch.ElapsedMilliseconds;
        Console.WriteLine($"Debuglogging duurde: {debugEnd - debugStart} ms");

        if (!inspecties.Any())
        {
            return Results.NotFound($"Geen aankomende inspecties gevonden voor inspecteur {inspecteurId}.");
        }

        if (includeMetadata == true)
        {
            var metadataStart = stopwatch.ElapsedMilliseconds;
            var fields = new List<object>
            {
                new { FieldName = "project", DisplayOrder = 1, ColumnWidth = "200px" },
                new { FieldName = "projectNr", DisplayOrder = 2, ColumnWidth = "100px" },
                new { FieldName = "adres", DisplayOrder = 3, ColumnWidth = "250px" },
                new { FieldName = "applicateur", DisplayOrder = 4, ColumnWidth = "150px" },
                new { FieldName = "soort", DisplayOrder = 5, ColumnWidth = "100px" },
                new { FieldName = "omschrijving", DisplayOrder = 6, ColumnWidth = "200px" },
                new { FieldName = "toegewezen", DisplayOrder = 7, ColumnWidth = "80px" },
                new { FieldName = "datumGereed", DisplayOrder = 8, ColumnWidth = "120px" },
                new { FieldName = "status", DisplayOrder = 9, ColumnWidth = "100px" }
            };
            var metadataEnd = stopwatch.ElapsedMilliseconds;
            Console.WriteLine($"Metadata aanmaken duurde: {metadataEnd - metadataStart} ms");

            stopwatch.Stop();
            Console.WriteLine($"Totale tijd met metadata: {stopwatch.ElapsedMilliseconds} ms");
            return Results.Ok(new { data = inspecties, fields });
        }

        stopwatch.Stop();
        Console.WriteLine($"Totale tijd zonder metadata: {stopwatch.ElapsedMilliseconds} ms");
        return Results.Ok(inspecties);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.ToString());
    }
})
.WithName("GetUpcomingInspections")
.WithOpenApi();

// Document Generate Endpoint
app.MapPost("/api/documents/generate/{inspectieId}", async (int inspectieId, ApplicationDbContext context, ILogger<Program> logger) =>
{
    var inspectie = await context.Inspecties
        .FirstOrDefaultAsync(i => i.OpdrachtId == inspectieId);
    if (inspectie == null) return Results.NotFound($"Inspectie met ID {inspectieId} niet gevonden.");

    var correspondentie = new Correspondentie
    {
        KlantID = inspectie.fldOpdrachtgeverId ?? 0,
        fldCorProjNum = inspectie.fldProjectId,
        fldCorOpdrachtNum = inspectie.fldOpdrachtId,
        fldCorAuteur = inspectie.ExtraMedewerker ?? inspectie.fldProjectLeider ?? "Onbekend",
        fldCorDatum = DateTime.Now,
        fldCorOmschrijving = inspectie.Omschrijving ?? "Inspectierapport",
        fldCorCPersId = inspectie.fldContactpersoonId
    };
    context.Correspondentie.Add(correspondentie);
    await context.SaveChangesAsync();

    string templatePath = @"M:\Projectdossier\sjablonen\adressjabloon.docx";
    string documentPath = $@"M:\Projectdossier\2025\documenten\rapport_{correspondentie.Id}.docx";

    try
    {
        string? directory = Path.GetDirectoryName(documentPath);
        if (directory != null && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        if (!File.Exists(templatePath))
            return Results.Problem($"Sjabloonbestand niet gevonden op: {templatePath}");

        File.Copy(templatePath, documentPath, true);

        using (WordprocessingDocument doc = WordprocessingDocument.Open(documentPath, true))
        {
            var customPropsPart = doc.CustomFilePropertiesPart;
            if (customPropsPart == null || customPropsPart.Properties == null)
            {
                logger.LogWarning("Geen custom properties gevonden in het sjabloon.");
                // Ga door met opslaan, zelfs als er geen properties zijn
            }
            else
            {
                var props = customPropsPart.Properties;

                // Log bestaande properties
                logger.LogInformation("Bestaande custom properties in het sjabloon:");
                foreach (var prop in props.Elements<DocumentFormat.OpenXml.CustomProperties.CustomDocumentProperty>())
                {
                    logger.LogInformation($"Naam: {prop.Name?.Value}, Waarde: {prop.InnerText}, Type: {prop.FirstChild?.LocalName}");
                }

                // Haal alle velddefinities uit StblCorrespondentieFields
                var fieldDefs = await context.StblCorrespondentieFields.ToListAsync();

                // Wijzig alleen properties die "adres" als tabel hebben
                foreach (var prop in props.Elements<DocumentFormat.OpenXml.CustomProperties.CustomDocumentProperty>())
                {
                    string propName = prop.Name?.Value ?? "";
                    if (string.IsNullOrEmpty(propName)) continue;

                    // Zoek de corresponderende velddefinitie
                    var fieldDef = fieldDefs.FirstOrDefault(f => f.ReplaceString == propName);
                    if (fieldDef == null)
                    {
                        logger.LogWarning($"Geen velddefinitie gevonden voor custom property '{propName}'. Property wordt genegeerd.");
                        continue;
                    }

                    // Verwerk alleen als Tabel="adres"
                    if (fieldDef.Tabel?.ToLower() != "adres")
                    {
                        logger.LogInformation($"Custom property '{propName}' gebruikt tabel '{fieldDef.Tabel}', wat niet 'adres' is. Property wordt genegeerd.");
                        continue;
                    }

                    // Haal de waarde op via de helper
                    string newValue = await CorrespondentieHelper.GetCorrespondentieVeldValueAsync(context, fieldDef, correspondentie);
                    if (string.IsNullOrEmpty(newValue))
                    {
                        logger.LogWarning($"Geen waarde gevonden voor custom property '{propName}'. Waarde wordt leeg gemaakt.");
                        newValue = "";
                    }

                    // Update de property-waarde
                    UpdatePropertyValue(prop, newValue);
                    logger.LogInformation($"Property '{propName}' gewijzigd naar: '{newValue}'");
                }

                customPropsPart.Properties.Save();
            }

            // Voeg de UpdateFields-instelling toe om velden automatisch bij te werken bij het openen
            UpdateFieldsInDocument(doc);

            // Sla het hele document expliciet op
            doc.MainDocumentPart?.Document.Save();
        }

        correspondentie.fldCorBestand = documentPath;
        await context.SaveChangesAsync();

        return Results.Ok(new { FilePath = documentPath, CorrespondentieID = correspondentie.Id });
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Fout bij genereren van document");
        return Results.Problem($"Fout bij genereren van document: {ex.Message}");
    }
})
.WithName("GenerateDocument")
.WithOpenApi();

// Methode om de UpdateFields-instelling toe te voegen
static void UpdateFieldsInDocument(WordprocessingDocument doc)
{
    // Toegang tot de DocumentSettingsPart
    var settingsPart = doc.MainDocumentPart.DocumentSettingsPart;
    if (settingsPart == null)
    {
        settingsPart = doc.MainDocumentPart.AddNewPart<DocumentSettingsPart>();
        settingsPart.Settings = new Settings();
    }

    // Controleer of de UpdateFields-instelling al bestaat
    var updateFields = settingsPart.Settings.Elements<UpdateFieldsOnOpen>().FirstOrDefault();
    if (updateFields == null)
    {
        updateFields = new UpdateFieldsOnOpen { Val = true };
        settingsPart.Settings.Append(updateFields);
    }
    else
    {
        updateFields.Val = true;
    }

    // Sla de instellingen op
    settingsPart.Settings.Save();
}

// Hulpmethode om de waarde van een custom property te updaten
static void UpdatePropertyValue(DocumentFormat.OpenXml.CustomProperties.CustomDocumentProperty prop, string newValue)
{
    if (prop.VTLPWSTR != null)
    {
        prop.RemoveChild(prop.VTLPWSTR); // Verwijder de oude VTLPWSTR
        prop.VTLPWSTR = new DocumentFormat.OpenXml.VariantTypes.VTLPWSTR(newValue ?? "");
    }
    else if (prop.VTInt32 != null)
    {
        int.TryParse(newValue, out int intValue);
        prop.RemoveChild(prop.VTInt32); // Verwijder de oude VTInt32
        prop.VTInt32 = new DocumentFormat.OpenXml.VariantTypes.VTInt32(intValue.ToString());
    }
    else if (prop.VTBool != null)
    {
        bool.TryParse(newValue, out bool boolValue);
        prop.RemoveChild(prop.VTBool); // Verwijder de oude VTBool
        prop.VTBool = new DocumentFormat.OpenXml.VariantTypes.VTBool(boolValue ? "1" : "0"); // 1 voor true, 0 voor false
    }
    else if (prop.VTDate != null)
    {
        if (DateTime.TryParse(newValue, out DateTime dateValue))
        {
            prop.RemoveChild(prop.VTDate); // Verwijder de oude VTDate
            prop.VTDate = new DocumentFormat.OpenXml.VariantTypes.VTDate(dateValue.ToString("O")); // Gebruik "O" (roundtrip) formaat
        }
        else
        {
            prop.RemoveChild(prop.VTDate);
            prop.VTDate = new DocumentFormat.OpenXml.VariantTypes.VTDate(DateTime.MinValue.ToString("O"));
        }
    }
    // Voeg meer typen toe als je sjabloon die gebruikt
}

app.UseExceptionMiddleware();

app.Run();

// Data Transfer Objects
public class LoginRequest
{
    public string? Voornaam { get; set; }
    public string? FldLoginNaam { get; set; }
}

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Een onverwerkte fout is opgetreden!");
            context.Response.StatusCode = 500;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = ex.Message }));
        }
    }
}

public static class ExceptionMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}