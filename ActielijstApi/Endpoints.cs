using ActielijstApi.Data;
using ActielijstApi.Dtos;
using ActielijstApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

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
                    //Console.WriteLine(JsonSerializer.Serialize(inspecties));
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
                return Results.Ok(); // Placeholder
            })
            .WithName("GenerateDocument")
            .WithOpenApi();

            app.MapGet("/api/assignmentLists", async (ApplicationDbContext context, ILogger<Program> logger, HttpContext httpContext) =>
            {
                var userRole = httpContext.User.FindFirst(ClaimTypes.Role)?.Value ?? "User";

                var lists = await context.AssignmentListDefinitions
                    .Where(l => context.AssignmentFieldConfigs.Any(c => c.ListDefinitionId == l.Id && (c.UserRole == null || c.UserRole == userRole)))
                    .OrderBy(l => l.SortOrder)
                    .ThenBy(l => l.ListName)
                    .Select(l => new { l.Id, l.ListName, l.Description })
                    .ToListAsync();

                return Results.Ok(lists);
            })
            .WithName("GetAssignmentLists")
            .WithOpenApi();

            app.MapGet("/api/genericList", async (ApplicationDbContext context, ILogger<Program> logger, HttpContext httpContext,
                string listName, int? projectId = null, int? customerId = null, int pageNumber = 1, int pageSize = 50, string? sortBy = null) =>
            {
                logger.LogInformation("Fetching generic list for listName={ListName}, projectId={ProjectId}, customerId={CustomerId}", listName, projectId, customerId);

                // Validatie
                if (string.IsNullOrEmpty(listName)) return Results.BadRequest("listName is verplicht!");
                if (pageNumber < 1) return Results.BadRequest("pageNumber moet groter dan 0 zijn!");
                if (pageSize < 1) return Results.BadRequest("pageSize moet groter dan 0 zijn!");

                var userRole = httpContext.User.FindFirst(ClaimTypes.Role)?.Value ?? "User";

                // Haal lijstdefinitie op
                var listDef = await context.AssignmentListDefinitions.FirstOrDefaultAsync(l => l.ListName == listName);
                if (listDef == null) return Results.NotFound($"Geen lijstdefinitie gevonden voor '{listName}'!");

                // Haal veldconfiguratie op
                var config = await context.AssignmentFieldConfigs
                    .Where(c => c.ListDefinitionId == listDef.Id && c.IsVisible && (c.UserRole == null || c.UserRole == userRole))
                    .OrderBy(c => c.DisplayOrder)
                    .ToListAsync();

                if (!config.Any()) return Results.BadRequest($"Geen zichtbare velden voor lijst '{listName}' en rol '{userRole}'!");

                // Query bouwen met joins
                var query = context.ProjectAssignments
                    .Include(pa => pa.Project)
                    .Include(pa => pa.Status)
                    .Include(pa => pa.Customer)
                    .Include(pa => pa.ProjectType)
                        .ThenInclude(pt => pt.RelatedCategory)
                    .AsQueryable();

                // Filters uit lijstdefinitie
                if (listDef.CategorieId.HasValue) query = query.Where(pa => pa.FldSoort == listDef.CategorieId.Value);
                if (!string.IsNullOrEmpty(listDef.FldAfdeling)) query = query.Where(pa => pa.FldAfdeling == listDef.FldAfdeling);

                // Extra filters
                if (projectId.HasValue) query = query.Where(pa => pa.FldProjectId == projectId);
                if (customerId.HasValue) query = query.Where(pa => pa.FldOpdrachtgeverId == customerId);

                // Filteren per kolom (handmatig parsen van queryparameters)
                var filter = new Dictionary<string, string>();
                foreach (var key in httpContext.Request.Query.Keys)
                {
                    if (key.StartsWith("filter[") && key.EndsWith("]"))
                    {
                        var fieldName = key.Substring(7, key.Length - 8); // Verwijdert "filter[" en "]"
                        var value = httpContext.Request.Query[key].ToString();
                        if (!string.IsNullOrEmpty(value))
                        {
                            filter[fieldName] = value;
                        }
                    }
                }

                if (filter.Any())
                {
                    foreach (var f in filter)
                    {
                        var fieldName = f.Key;
                        var filterValue = f.Value?.ToLower();
                        if (!string.IsNullOrEmpty(filterValue) && config.Any(c => c.FieldName == fieldName && c.IsFilterable))
                        {
                            switch (fieldName)
                            {
                                case "Id":
                                    if (int.TryParse(filterValue, out var id))
                                        query = query.Where(pa => pa.Id == id);
                                    break;
                                case "FldOpdrachtStr":
                                    query = query.Where(pa => pa.FldOpdrachtStr != null && pa.FldOpdrachtStr.ToLower().Contains(filterValue));
                                    break;
                                case "AantalM2":
                                    if (int.TryParse(filterValue, out var aantalM2))
                                        query = query.Where(pa => pa.Project != null && pa.Project.FldAantalM2 == aantalM2);
                                    break;
                                case "FldCertKeuring":
                                    if (bool.TryParse(filterValue, out var certKeuring))
                                        query = query.Where(pa => pa.FldCertKeuring == certKeuring);
                                    break;
                                case "OpdrachtLocatie":
                                    query = query.Where(pa => pa.OpdrachtPlaats != null && (pa.OpdrachtPlaats + " (" + pa.OpdrachtAdres + ")").ToLower().Contains(filterValue));
                                    break;
                                case "Applicator":
                                    query = query.Where(pa => pa.Project != null && pa.Project.FldFabrikant != null && pa.Project.FldFabrikant.ToLower().Contains(filterValue));
                                    break;
                                case "FldPlanDatum":
                                    if (DateTime.TryParse(filterValue, out var planDatum))
                                        query = query.Where(pa => pa.FldPlanDatum != null && pa.FldPlanDatum == planDatum);
                                    break;
                                case "KiwaNumber":
                                    query = query.Where(pa => pa.Project != null && pa.Project.FldKiWa != null && pa.Project.FldKiWa.ToLower().Contains(filterValue));
                                    break;
                                case "StatusName":
                                    query = query.Where(pa => pa.Status != null && pa.Status.StatusName != null && pa.Status.StatusName.ToLower().Contains(filterValue));
                                    break;
                                case "FldDatumGereed":
                                    if (DateTime.TryParse(filterValue, out var datumGereed))
                                        query = query.Where(pa => pa.FldDatumGereed != null && pa.FldDatumGereed == datumGereed);
                                    break;
                                case "BelNotitie":
                                    query = query.Where(pa => pa.BelNotitie != null && pa.BelNotitie.ToLower().Contains(filterValue));
                                    break;
                                case "FldProjectLeider":
                                    query = query.Where(pa => pa.FldProjectLeider != null && pa.FldProjectLeider.ToLower().Contains(filterValue));
                                    break;
                                case "ExtraMedewerker":
                                    query = query.Where(pa => pa.ExtraMedewerker != null && pa.ExtraMedewerker.ToLower().Contains(filterValue));
                                    break;
                                case "FldKiwaKeuringsNr":
                                    if (int.TryParse(filterValue, out var kiwaKeuringsNr))
                                        query = query.Where(pa => pa.FldKiwaKeuringsNr == kiwaKeuringsNr);
                                    break;
                            }
                        }
                    }
                }

                // Totaal aantal items
                var totalCount = await query.CountAsync();

                // Sortering
                if (!string.IsNullOrEmpty(sortBy))
                {
                    var sortField = sortBy.Split(' ')[0];
                    var sortDirection = sortBy.EndsWith(" desc") ? "desc" : "asc";
                    if (config.Any(f => f.FieldName == sortField && f.IsSortable))
                    {
                        switch (sortField)
                        {
                            case "Id":
                                query = sortDirection == "desc" ?
                                    query.OrderByDescending(pa => pa.Id) :
                                    query.OrderBy(pa => pa.Id);
                                break;
                            case "FldOpdrachtStr":
                                query = sortDirection == "desc" ?
                                    query.OrderByDescending(pa => pa.FldOpdrachtStr) :
                                    query.OrderBy(pa => pa.FldOpdrachtStr);
                                break;
                            case "AantalM2":
                                query = sortDirection == "desc" ?
                                    query.OrderByDescending(pa => pa.Project != null ? pa.Project.FldAantalM2 : null) :
                                    query.OrderBy(pa => pa.Project != null ? pa.Project.FldAantalM2 : null);
                                break;
                            case "FldCertKeuring":
                                query = sortDirection == "desc" ?
                                    query.OrderByDescending(pa => pa.FldCertKeuring) :
                                    query.OrderBy(pa => pa.FldCertKeuring);
                                break;
                            case "OpdrachtLocatie":
                                query = sortDirection == "desc" ?
                                    query.OrderByDescending(pa => pa.OpdrachtPlaats != null ? pa.OpdrachtPlaats + " (" + pa.OpdrachtAdres + ")" : null) :
                                    query.OrderBy(pa => pa.OpdrachtPlaats != null ? pa.OpdrachtPlaats + " (" + pa.OpdrachtAdres + ")" : null);
                                break;
                            case "Applicator":
                                query = sortDirection == "desc" ?
                                    query.OrderByDescending(pa => pa.Project != null ? pa.Project.FldFabrikant : null) :
                                    query.OrderBy(pa => pa.Project != null ? pa.Project.FldFabrikant : null);
                                break;
                            case "FldPlanDatum":
                                query = sortDirection == "desc" ?
                                    query.OrderByDescending(pa => pa.FldPlanDatum) :
                                    query.OrderBy(pa => pa.FldPlanDatum);
                                break;
                            case "KiwaNumber":
                                query = sortDirection == "desc" ?
                                    query.OrderByDescending(pa => pa.Project != null ? pa.Project.FldKiWa : null) :
                                    query.OrderBy(pa => pa.Project != null ? pa.Project.FldKiWa : null);
                                break;
                            case "StatusName":
                                query = sortDirection == "desc" ?
                                    query.OrderByDescending(pa => pa.Status != null ? pa.Status.StatusName : null) :
                                    query.OrderBy(pa => pa.Status != null ? pa.Status.StatusName : null);
                                break;
                            case "FldDatumGereed":
                                query = sortDirection == "desc" ?
                                    query.OrderByDescending(pa => pa.FldDatumGereed) :
                                    query.OrderBy(pa => pa.FldDatumGereed);
                                break;
                            case "BelNotitie":
                                query = sortDirection == "desc" ?
                                    query.OrderByDescending(pa => pa.BelNotitie) :
                                    query.OrderBy(pa => pa.BelNotitie);
                                break;
                            case "FldProjectLeider":
                                query = sortDirection == "desc" ?
                                    query.OrderByDescending(pa => pa.FldProjectLeider) :
                                    query.OrderBy(pa => pa.FldProjectLeider);
                                break;
                            case "ExtraMedewerker":
                                query = sortDirection == "desc" ?
                                    query.OrderByDescending(pa => pa.ExtraMedewerker) :
                                    query.OrderBy(pa => pa.ExtraMedewerker);
                                break;
                            case "FldKiwaKeuringsNr":
                                query = sortDirection == "desc" ?
                                    query.OrderByDescending(pa => pa.FldKiwaKeuringsNr) :
                                    query.OrderBy(pa => pa.FldKiwaKeuringsNr);
                                break;
                            default:
                                query = query.OrderBy(pa => pa.Id);
                                break;
                        }
                    }
                    else
                    {
                        query = query.OrderBy(pa => pa.Id);
                    }
                }
                else
                {
                    query = query.OrderBy(pa => pa.Id);
                }

                // Paginering
                var assignments = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .Select(pa => new Dictionary<string, object>
                    {
                        { "Id", pa.Id },
                        { "FldOpdrachtStr", pa.FldOpdrachtStr },
                        { "AantalM2", pa.Project != null ? pa.Project.FldAantalM2 : null },
                        { "FldCertKeuring", pa.FldCertKeuring },
                        { "OpdrachtLocatie", pa.OpdrachtPlaats != null ? $"{pa.OpdrachtPlaats} ({pa.OpdrachtAdres})" : null },
                        { "Applicator", pa.Project != null ? pa.Project.FldFabrikant : null },
                        { "FldPlanDatum", pa.FldPlanDatum },
                        { "KiwaNumber", pa.Project != null ? pa.Project.FldKiWa : null },
                        { "StatusName", pa.Status != null ? pa.Status.StatusName : null },
                        { "FldDatumGereed", pa.FldDatumGereed },
                        { "BelNotitie", pa.BelNotitie },
                        { "FldProjectLeider", pa.FldProjectLeider },
                        { "ExtraMedewerker", pa.ExtraMedewerker },
                        { "FldKiwaKeuringsNr", pa.FldKiwaKeuringsNr }
                    })
                    .ToListAsync();

                // Pas achtergrondkleur toe op basis van BackgroundColorRule
                foreach (var assignment in assignments)
                {
                    foreach (var field in config.Where(f => !string.IsNullOrEmpty(f.BackgroundColorRule)))
                    {
                        if (field.BackgroundColorRule == "FldStatus == 1" && assignment["FldStatus"]?.ToString() == "1")
                        {
                            assignment[$"{field.FieldName}_BackgroundColor"] = "lightcoral";
                        }
                    }
                }

                return Results.Ok(new
                {
                    totalCount,
                    pageNumber,
                    pageSize,
                    listName,
                    data = assignments,
                    fields = config.Select(c => new
                    {
                        c.FieldName,
                        c.Prompt,
                        c.DataType,
                        c.IsSortable,
                        c.IsFilterable,
                        c.FormatString,
                        c.Width,
                        c.BackgroundColorRule
                    })
                });
            })
            .WithName("GetGenericList")
            .WithOpenApi();

            // Nieuwe /api/adres endpoint
            app.MapGet("/api/adres", async (ApplicationDbContext context, ILogger<Program> logger) =>
            {
                try
                {
                    var klanten = await context.Adressen
                        .Select(a => new Adres
                        {
                            Id = a.Id,
                            Bedrijf = a.Bedrijf
                        })
                        .ToListAsync();
                    return Results.Ok(klanten);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Fout bij het ophalen van klanten.");
                    return Results.Problem("Kon klanten niet ophalen: " + ex.Message);
                }
            })
            .WithName("GetKlanten")
            .WithOpenApi();

            // Hulp methode voor achtergrondkleur (ongewijzigd)
            static string EvaluateBackgroundColor(string rule, ProjectAssignment pa, string fieldName)
            {
                try
                {
                    var parts = rule.Split('?');
                    if (parts.Length != 2) return "";
                    var condition = parts[0].Trim();
                    var outcomes = parts[1].Split(':');
                    if (outcomes.Length != 2) return "";

                    var trueColor = outcomes[0].Trim().Trim('\'');
                    var falseColor = outcomes[1].Trim().Trim('\'');

                    var property = typeof(ProjectAssignment).GetProperty(fieldName);
                    var value = property?.GetValue(pa);

                    if (condition.Contains("< GETDATE()") && value is DateTime date)
                        return date < DateTime.Now ? trueColor : falseColor;

                    return "";
                }
                catch
                {
                    return "";
                }
            }

            app.MapGet("/api/projectAssignments", async (ApplicationDbContext context, ILogger<Program> logger,
                int? projectId = null, int? categoryId = null, int? assignmentType = null, string? department = null,
                int pageNumber = 1, int pageSize = 50, string? searchTerm = null, string? sortBy = null, string? sortDirection = "asc") =>
            {
                try
                {
                    var stopwatch = System.Diagnostics.Stopwatch.StartNew();

                    var query = context.ProjectAssignments
                        .Include(pa => pa.ProjectType)
                            .ThenInclude(pt => pt.RelatedCategory)
                        .Include(pa => pa.Project)
                            .ThenInclude(p => p.VerwerkendBedrijf)
                        .Include(pa => pa.Status)
                        .Include(pa => pa.Customer)
                        .Select(pa => new ProjectAssignmentDto
                        {
                            Id = pa.Id,
                            FldProjectId = pa.FldProjectId,
                            FldSoort = pa.FldSoort,
                            CategoryId = pa.ProjectType != null ? pa.ProjectType.CategorieId : (int?)null,
                            CategoryName = pa.ProjectType != null && pa.ProjectType.CategorieName != null ? pa.ProjectType.CategorieName : null,
                            AssignmentTypeName = pa.ProjectType != null ? pa.ProjectType.Omschrijving : null,
                            Department = pa.Project != null ? pa.Project.FldAfdeling : null,
                            FldOmschrijving = pa.FldOmschrijving,
                            FldOpdrachtStr = pa.FldOpdrachtStr,
                            FldStatus = pa.FldStatus,
                            StatusName = pa.Status != null ? pa.Status.StatusName : null,
                            FldPlanDatum = pa.FldPlanDatum,
                            FldBedrag = pa.FldBedrag,
                            FldKiwabedrag = pa.FldKiwabedrag,
                            FldMaandBedrag = pa.FldMaandBedrag,
                            FldProjectLeider = pa.FldProjectLeider,
                            ExtraMedewerker = pa.ExtraMedewerker,
                            FldDatumGereed = pa.FldDatumGereed,
                            FldOpdrachtgeverId = pa.FldOpdrachtgeverId,
                            FldContactpersoonId = pa.FldContactpersoonId,
                            FldAantalKms = pa.FldAantalKms,
                            FldKmvergoeding = pa.FldKmvergoeding,
                            Fabrikant = pa.Fabrikant,
                            Systeem = pa.Systeem,
                            AantalM2 = pa.Project != null ? pa.Project.FldAantalM2 : null,
                            Gnummer = pa.Gnummer,
                            Datum1eInspectie1 = pa.Datum1eInspectie1,
                            Contractnr = pa.Contractnr,
                            Looptijd = pa.Looptijd,
                            EindDatumContract = pa.EindDatumContract,
                            Factuurmaand = pa.Factuurmaand,
                            FldCertKeuring = pa.FldCertKeuring,
                            FldKiwaKeuringsNr = pa.FldKiwaKeuringsNr,
                            Kortingbedrag = pa.Kortingbedrag,
                            Kortingspercentage = pa.Kortingspercentage,
                            AppointmentDateTime = pa.AppointmentDateTime,
                            OpdrachtAdres = pa.OpdrachtAdres,
                            OpdrachtHuisnr = pa.OpdrachtHuisnr,
                            OpdrachtPC = pa.OpdrachtPC,
                            OpdrachtPlaats = pa.OpdrachtPlaats,
                            OpdrachtLocatie = $"{pa.OpdrachtAdres ?? ""} {pa.OpdrachtHuisnr ?? ""} {(pa.OpdrachtPlaats != null ? $"({pa.OpdrachtPlaats})" : "")}".Trim(),
                            ContractBedrag = pa.ContractBedrag,
                            ContractIndexering = pa.ContractIndexering,
                            FldPlanPeriodeVan = pa.FldPlanPeriodeVan,
                            FldPlanPeriodeTot = pa.FldPlanPeriodeTot,
                            SteekproefMaand = pa.SteekproefMaand,
                            ProjectName = pa.Project != null ? pa.Project.FldProjectNaam : null,
                            ProjectLocation = pa.Project != null ? pa.Project.FldPlaats + " (" + pa.Project.FldAdres + ")" : null,
                            CustomerName = pa.Customer != null ? pa.Customer.Bedrijf : null,
                            Applicator = pa.Project != null && pa.Project.VerwerkendBedrijf != null ? pa.Project.VerwerkendBedrijf.ZOEKCODE : null,
                            KiwaNumber = pa.Customer != null ? pa.Customer.KiwaNummer : null
                        });

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
                        var searchTermLower = searchTerm.ToLower();
                        query = query.Where(a =>
                            (a.FldOmschrijving != null && a.FldOmschrijving.ToLower().Contains(searchTermLower)) ||
                            (a.FldOpdrachtStr != null && a.FldOpdrachtStr.ToLower().Contains(searchTermLower)) ||
                            (a.OpdrachtAdres != null && a.OpdrachtAdres.ToLower().Contains(searchTermLower)) ||
                            (a.OpdrachtHuisnr != null && a.OpdrachtHuisnr.ToLower().Contains(searchTermLower)) ||
                            (a.OpdrachtPlaats != null && a.OpdrachtPlaats.ToLower().Contains(searchTermLower)) ||
                            (a.OpdrachtLocatie != null && a.OpdrachtLocatie.ToLower().Contains(searchTermLower)) ||
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
                        bool isDescending = sortDirection?.ToLower() == "desc";
                        query = sortBy.ToLower() switch
                        {
                            "name" => isDescending
                                ? query.OrderByDescending(a => a.FldOmschrijving)
                                : query.OrderBy(a => a.FldOmschrijving),
                            "assignmentnumber" => isDescending
                                ? query.OrderByDescending(a => a.FldOpdrachtStr)
                                : query.OrderBy(a => a.FldOpdrachtStr),
                            "status" => isDescending
                                ? query.OrderByDescending(a => a.FldStatus ?? int.MinValue)
                                : query.OrderBy(a => a.FldStatus ?? int.MinValue),
                            "projectleider" => isDescending
                                ? query.OrderByDescending(a => a.FldProjectLeider ?? "")
                                : query.OrderBy(a => a.FldProjectLeider ?? ""),
                            "extramedewerker" => isDescending
                                ? query.OrderByDescending(a => a.ExtraMedewerker ?? "")
                                : query.OrderBy(a => a.ExtraMedewerker ?? ""),
                            "aanmaakdatum" => isDescending
                                ? query.OrderByDescending(a => a.FldPlanDatum ?? DateTime.MinValue)
                                : query.OrderBy(a => a.FldPlanDatum ?? DateTime.MinValue),
                            "gereeddatum" => isDescending
                                ? query.OrderByDescending(a => a.FldDatumGereed ?? DateTime.MinValue)
                                : query.OrderBy(a => a.FldDatumGereed ?? DateTime.MinValue),
                            "applicator" => isDescending
                                ? query.OrderByDescending(a => a.Applicator ?? "")
                                : query.OrderBy(a => a.Applicator ?? ""),
                            "statusname" => isDescending
                                ? query.OrderByDescending(a => a.StatusName ?? "")
                                : query.OrderBy(a => a.StatusName ?? ""),
                            "projectname" => isDescending
                                ? query.OrderByDescending(a => a.ProjectName ?? "")
                                : query.OrderBy(a => a.ProjectName ?? ""),
                            "projectnumber" => isDescending
                                ? query.OrderByDescending(a => a.ProjectNumber ?? "")
                                : query.OrderBy(a => a.ProjectNumber ?? ""),
                            "projectlocation" => isDescending
                                ? query.OrderByDescending(a => a.ProjectLocation ?? "")
                                : query.OrderBy(a => a.ProjectLocation ?? ""),
                            "customname" => isDescending
                                ? query.OrderByDescending(a => a.CustomerName ?? "")
                                : query.OrderBy(a => a.CustomerName ?? ""),
                            "opdrachtlocatie" => isDescending
                                ? query.OrderByDescending(a => a.OpdrachtLocatie ?? "")
                                : query.OrderBy(a => a.OpdrachtLocatie ?? ""),
                            "kiwanumber" => isDescending
                                ? query.OrderByDescending(a => a.KiwaNumber ?? "")
                                : query.OrderBy(a => a.KiwaNumber ?? ""),
                            _ => isDescending
                                ? query.OrderByDescending(a => a.Id)
                                : query.OrderBy(a => a.Id)
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