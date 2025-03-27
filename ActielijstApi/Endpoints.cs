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
using ActielijstApi.Dtos;

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

            app.MapGet("/api/priorities", async (ApplicationDbContext context) =>
            {
                try
                {
                    var priorities = await context.Priorities.ToListAsync(); // Pas aan naar juiste tabelnaam
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
                // Bestaande logica behouden
                try
                {
                    var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                    if (string.IsNullOrEmpty(inspecteurId))
                    {
                        return Results.BadRequest("InspecteurId is verplicht.");
                    }

                    var queryStart = stopwatch.ElapsedMilliseconds;
                    var inspecties = await context.AankomendeInspecties // Pas aan naar juiste tabelnaam
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
                // Bestaande logica behouden
                // ...
            })
            .WithName("GenerateDocument")
            .WithOpenApi();
        }
    }
}