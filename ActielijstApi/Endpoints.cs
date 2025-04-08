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
                    var priorities = await context.Priorities.ToListAsync();
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
                    return Results.Problem(ex.ToString() ?? "Er is een onbekende fout opgetreden.");
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

            // Nieuwe endpoint voor projectAssignments
            app.MapGet("/api/projectAssignments", async (ApplicationDbContext context, ILogger<Program> logger,
                int? projectId = null, int? categoryId = null, int? assignmentType = null, string? department = null,
                int pageNumber = 1, int pageSize = 50, string? searchTerm = null, string? sortBy = null) =>
            {
                try
                {
                    var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                    var queryStart = stopwatch.ElapsedMilliseconds;
                    var query = from po in context.ProjectAssignments
                                join sp in context.ProjectTypes on po.FldSoort equals sp.Id into spJoin
                                from sp in spJoin.DefaultIfEmpty()
                                join oc in context.AssignmentCategories on sp.CategorieId equals oc.Id into ocJoin
                                from oc in ocJoin.DefaultIfEmpty()
                                join p in context.Projecten on po.FldProjectId equals p.Id into pJoin
                                from p in pJoin.DefaultIfEmpty()
                                join s in context.Statuses on po.FldStatus equals s.Id into sJoin
                                from s in sJoin.DefaultIfEmpty()
                                join a in context.Adressen on po.FldOpdrachtgeverId equals a.Id into aJoin
                                from a in aJoin.DefaultIfEmpty()
                                select new ProjectAssignmentDto
                                {
                                    Id = po.Id,
                                    FldProjectId = po.FldProjectId,
                                    FldSoort = po.FldSoort,
                                    CategoryId = sp != null ? sp.CategorieId : (int?)null,
                                    CategoryName = oc != null ? oc.Categorie : null,
                                    AssignmentTypeName = sp != null ? sp.Omschrijving : null,
                                    Department = p != null ? p.FldAfdeling : null,
                                    FldOmschrijving = po.FldOmschrijving,
                                    FldOpdrachtStr = po.FldOpdrachtStr,
                                    FldStatus = po.FldStatus,
                                    StatusName = s != null ? s.StatusName : null,
                                    FldPlanDatum = po.FldPlanDatum,
                                    FldBedrag = po.FldBedrag,
                                    FldKiwabedrag = po.FldKiwabedrag,
                                    FldMaandBedrag = po.FldMaandBedrag,
                                    FldProjectLeider = po.FldProjectLeider,
                                    ExtraMedewerker = po.ExtraMedewerker,
                                    FldDatumGereed = po.FldDatumGereed,
                                    FldOpdrachtgeverId = po.FldOpdrachtgeverId,
                                    FldContactpersoonId = po.FldContactpersoonId,
                                    FldAantalKms = po.FldAantalKms,
                                    FldKmvergoeding = po.FldKmvergoeding,
                                    Fabrikant = po.Fabrikant,
                                    Systeem = po.Systeem,
                                    AantalM2 = p != null && p.FldAantalM2.HasValue ? p.FldAantalM2.Value.ToString() : null,
                                    Gnummer = po.Gnummer,
                                    Datum1eInspectie1 = po.Datum1eInspectie1,
                                    Contractnr = po.Contractnr,
                                    Looptijd = po.Looptijd,
                                    EindDatumContract = po.EindDatumContract,
                                    Factuurmaand = po.Factuurmaand,
                                    FldCertKeuring = po.FldCertKeuring,
                                    FldKiwaKeuringsNr = po.FldKiwaKeuringsNr,
                                    Kortingbedrag = po.Kortingbedrag,
                                    Kortingspercentage = po.Kortingspercentage,
                                    AppointmentDateTime = po.AppointmentDateTime,
                                    OpdrachtAdres = po.OpdrachtAdres,
                                    OpdrachtHuisnr = po.OpdrachtHuisnr,
                                    OpdrachtPC = po.OpdrachtPC,
                                    OpdrachtPlaats = po.OpdrachtPlaats,
                                    OpdrachtLocatie = $"{po.OpdrachtAdres ?? ""} {(po.OpdrachtHuisnr ?? "")} {(po.OpdrachtPlaats != null ? $"({po.OpdrachtPlaats})" : "")}".Trim(),
                                    ContractBedrag = po.ContractBedrag,
                                    ContractIndexering = po.ContractIndexering,
                                    FldPlanPeriodeVan = po.FldPlanPeriodeVan,
                                    FldPlanPeriodeTot = po.FldPlanPeriodeTot,
                                    SteekproefMaand = po.SteekproefMaand,
                                    ProjectName = p != null ? p.FldProjectNaam : null,
                                    ProjectNumber = p != null && p.FldProjectNummer.HasValue ? p.FldProjectNummer.Value.ToString() : null, // Oplossing voor CS0029
                                    ProjectLocation = p != null ? p.FldPlaats + " (" + p.FldAdres + ")" : null,
                                    CustomerName = a != null ? a.Bedrijf : null,
                                    Applicator = a != null ? a.ZOEKCODE : null,
                                    KiwaNumber = a != null ? a.KiwaNummer : null
                                };

                    // Filters
                    if (projectId.HasValue)
                        query = query.Where(a => a.FldProjectId == projectId);
                    if (categoryId.HasValue)
                        query = query.Where(a => a.CategoryId == categoryId);
                    if (assignmentType.HasValue)
                        query = query.Where(a => a.FldSoort.HasValue && a.FldSoort == assignmentType);
                    if (!string.IsNullOrEmpty(department))
                        query = query.Where(a => a.Department == department);
                    if (!string.IsNullOrEmpty(searchTerm))
                    {
                        // Maak de zoekterm lowercase voor case-insensitive vergelijking
                        var searchTermLower = searchTerm.ToLower();
                        query = query.Where(a =>
                            (a.FldOmschrijving != null && a.FldOmschrijving.ToLower().Contains(searchTermLower)) ||
                            (a.FldOpdrachtStr != null && a.FldOpdrachtStr.ToLower().Contains(searchTermLower)) ||
                            (a.OpdrachtAdres != null && a.OpdrachtAdres.ToLower().Contains(searchTermLower)) ||
                            (a.OpdrachtHuisnr != null && a.OpdrachtHuisnr.ToLower().Contains(searchTermLower)) || // Toegevoegd
                            (a.OpdrachtPlaats != null && a.OpdrachtPlaats.ToLower().Contains(searchTermLower)) ||
                            // We laten OpdrachtLocatie weg vanwege de string.Format-problemen
                            (a.ProjectName != null && a.ProjectName.ToLower().Contains(searchTermLower)) ||
                            (a.ProjectNumber != null && a.ProjectNumber.ToLower().Contains(searchTermLower)) ||
                            (a.ProjectLocation != null && a.ProjectLocation.ToLower().Contains(searchTermLower)) ||
                            (a.CustomerName != null && a.CustomerName.ToLower().Contains(searchTermLower)) ||
                            (a.Applicator != null && a.Applicator.ToLower().Contains(searchTermLower)) ||
                            (a.KiwaNumber != null && a.KiwaNumber.ToLower().Contains(searchTermLower))
                        );
                    }

                    // Sortering
                    if (!string.IsNullOrEmpty(sortBy))
                    {
                        query = sortBy switch
                        {
                            "name" => query.OrderBy(a => a.FldOmschrijving),
                            "assignmentNumber" => query.OrderBy(a => a.FldOpdrachtStr),
                            "status" => query.OrderBy(a => a.FldStatus.HasValue ? a.FldStatus : int.MinValue), // Null-check toegevoegd
                            _ => query.OrderBy(a => a.Id)
                        };
                    }

                    var countStart = stopwatch.ElapsedMilliseconds;
                    var totalCount = await query.CountAsync();
                    var countEnd = stopwatch.ElapsedMilliseconds;
                    logger.LogInformation($"Count query duurde: {countEnd - countStart} ms");

                    var dataStart = stopwatch.ElapsedMilliseconds;
                    var assignments = await query
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToListAsync();
                    var dataEnd = stopwatch.ElapsedMilliseconds;
                    logger.LogInformation($"Data query duurde: {dataEnd - dataStart} ms");

                    if (!assignments.Any())
                    {
                        stopwatch.Stop();
                        logger.LogInformation($"Totale tijd: {stopwatch.ElapsedMilliseconds} ms");
                        return Results.NotFound("Geen opdrachten gevonden.");
                    }

                    stopwatch.Stop();
                    logger.LogInformation($"Totale tijd: {stopwatch.ElapsedMilliseconds} ms");
                    return Results.Ok(new { Data = assignments, TotalCount = totalCount });
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Fout bij het ophalen van projectopdrachten");
                    return Results.Problem(ex.ToString() ?? "Er is een onbekende fout opgetreden.");
                }
            })
            .WithName("GetProjectAssignments")
            .WithOpenApi();
        }
    }
}