using ActielijstApi.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.IO;
using ActielijstApi.Models;
using Microsoft.AspNetCore.Http;
using ActielijstApi.Helpers;
using ActielijstApi.Dtos; // Toevoegen
using ActielijstApi.Services; // Toevoegen

namespace ActielijstApi
{
    public static class Endpoints
    {
        public static void RegisterEndpoints(this WebApplication app)
        {
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

            app.MapGet("/api/werknemers", async (ApplicationDbContext context) =>
            {
                var workers = await context.Werknemers.ToListAsync();
                return Results.Ok(workers);
            })
            .WithName("GetWerknemers")
            .WithOpenApi();

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

                var fields = await context.CorrespondentieFields
                    .FirstOrDefaultAsync(cf => cf.CorrespondentieNr == correspondentie.Id);
                if (fields == null)
                {
                    logger.LogWarning($"Geen gegevens gevonden in vw_CorrespondentieFields voor CorrespondentieNr {correspondentie.Id}");
                }

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
                            customPropsPart = doc.AddCustomFilePropertiesPart();
                            customPropsPart.Properties = new DocumentFormat.OpenXml.CustomProperties.Properties();
                        }

                        var props = customPropsPart.Properties;

                        logger.LogInformation("Bestaande custom properties in het sjabloon:");
                        foreach (var prop in props.Elements<DocumentFormat.OpenXml.CustomProperties.CustomDocumentProperty>())
                        {
                            logger.LogInformation($"Naam: {prop.Name?.Value}, Waarde: {prop.InnerText}, Type: {prop.FirstChild?.LocalName}");
                        }

                        if (fields != null)
                        {
                            foreach (var prop in props.Elements<DocumentFormat.OpenXml.CustomProperties.CustomDocumentProperty>())
                            {
                                string? propName = prop.Name?.Value;
                                if (string.IsNullOrEmpty(propName)) continue;

                                string? fieldValue = typeof(CorrespondentieFields).GetProperty(propName)?.GetValue(fields)?.ToString();
                                if (fieldValue != null)
                                {
                                    DocumentHelper.UpdatePropertyValue(prop, fieldValue);
                                    logger.LogInformation($"Property '{propName}' gewijzigd naar: '{fieldValue}'");
                                }
                                else
                                {
                                    logger.LogWarning($"Geen waarde gevonden in vw_CorrespondentieFields voor property '{propName}'");
                                }
                            }
                        }
                        else
                        {
                            logger.LogWarning("Geen velden beschikbaar om custom properties bij te werken.");
                        }

                        customPropsPart.Properties.Save();

                        DocumentHelper.UpdateFieldsInDocument(doc);

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

            // Nieuw endpoint: /api/correspondence/generate
            app.MapPost("/api/correspondence/generate", async (GenerateCorrespondenceRequest request, CorrespondenceService service) =>
            {
                try
                {
                    var response = await service.GenerateCorrespondenceAsync(request);
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    return Results.Problem($"Fout bij genereren van correspondentie: {ex.Message}");
                }
            })
            .WithName("GenerateCorrespondence")
            .WithOpenApi();
        }
    }
}