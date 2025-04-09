using ActielijstApi.Data;
using ActielijstApi.Dtos;
using ActielijstApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
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
                string listName, int? projectId = null, int pageNumber = 1, int pageSize = 50, string? searchTerm = null, string? sortBy = null) =>
            {
                logger.LogInformation("Fetching generic list for listName={ListName}, projectId={ProjectId}", listName, projectId);

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

                // Valideer veldnamen
                var validFields = typeof(ProjectAssignmentDto).GetProperties().Select(p => p.Name).ToHashSet();
                var invalidFields = config.Where(c => !validFields.Contains(c.FieldName)).Select(c => c.FieldName).ToList();
                if (invalidFields.Any()) return Results.BadRequest($"Ongeldige veldnamen: {string.Join(", ", invalidFields)}");

                // Query bouwen met joins
                var query = context.ProjectAssignments
                    .Include(pa => pa.Project)
                        .ThenInclude(p => p.VerwerkendBedrijf)
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
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(pa => config.Where(c => c.IsFilterable)
                        .Any(c =>
                            (c.FieldName == "Applicator" && pa.Project != null && pa.Project.VerwerkendBedrijf != null && pa.Project.VerwerkendBedrijf.ZOEKCODE != null && pa.Project.VerwerkendBedrijf.ZOEKCODE.Contains(searchTerm)) ||
                            (c.FieldName == "StatusName" && pa.Status != null && pa.Status.StatusName != null && pa.Status.StatusName.Contains(searchTerm)) ||
                            (c.FieldName == "ProjectName" && pa.Project != null && pa.Project.FldProjectNaam != null && pa.Project.FldProjectNaam.Contains(searchTerm)) ||
                            (c.FieldName == "ProjectLocation" && pa.Project != null && pa.Project.FldPlaats != null && (pa.Project.FldPlaats + " (" + pa.Project.FldAdres + ")").Contains(searchTerm)) ||
                            (c.FieldName == "CustomerName" && pa.Customer != null && pa.Customer.Bedrijf != null && pa.Customer.Bedrijf.Contains(searchTerm)) ||
                            (c.FieldName == "OpdrachtLocatie" && (pa.OpdrachtAdres + " " + pa.OpdrachtHuisnr + " (" + pa.OpdrachtPlaats + ")").Contains(searchTerm)) ||
                            (EF.Property<string>(pa, c.FieldName) != null && EF.Property<string>(pa, c.FieldName).Contains(searchTerm))
                        ));
                }

                // Sortering
                if (!string.IsNullOrEmpty(sortBy))
                {
                    var sortField = config.FirstOrDefault(c => c.FieldName.ToLower() == sortBy.ToLower() && c.IsSortable);
                    if (sortField != null)
                    {
                        query = sortField.FieldName.ToLower() switch
                        {
                            "applicator" => query.OrderBy(pa => pa.Project != null && pa.Project.VerwerkendBedrijf != null ? pa.Project.VerwerkendBedrijf.ZOEKCODE : ""),
                            "statusname" => query.OrderBy(pa => pa.Status != null ? pa.Status.StatusName : ""),
                            "projectname" => query.OrderBy(pa => pa.Project != null ? pa.Project.FldProjectNaam : ""),
                            "projectlocation" => query.OrderBy(pa => pa.Project != null ? pa.Project.FldPlaats + " (" + pa.Project.FldAdres + ")" : ""),
                            "customname" => query.OrderBy(pa => pa.Customer != null ? pa.Customer.Bedrijf : ""),
                            "opdrachtlocatie" => query.OrderBy(pa => pa.OpdrachtAdres + " " + pa.OpdrachtHuisnr + " (" + pa.OpdrachtPlaats + ")"),
                            _ => query.OrderBy(pa => EF.Property<object>(pa, sortField.FieldName))
                        };
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

                // Totaal aantal items
                var totalCount = await query.CountAsync();

                // Paginering
                var assignments = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // Dynamische response
                var result = assignments.Select(pa =>
                {
                    var dict = new Dictionary<string, object>();
                    foreach (var field in config)
                    {
                        object value;
                        switch (field.FieldName.ToLower())
                        {
                            case "applicator":
                                value = pa.Project?.VerwerkendBedrijf?.ZOEKCODE;
                                break;
                            case "statusname":
                                value = pa.Status?.StatusName;
                                break;
                            case "projectname":
                                value = pa.Project?.FldProjectNaam;
                                break;
                            case "projectnumber":
                                value = pa.Project?.FldProjectNummer;
                                break;
                            case "projectlocation":
                                value = pa.Project != null ? $"{pa.Project.FldPlaats} ({pa.Project.FldAdres})" : null;
                                break;
                            case "customname":
                                value = pa.Customer?.Bedrijf;
                                break;
                            case "opdrachtlocatie":
                                value = $"{pa.OpdrachtAdres ?? ""} {pa.OpdrachtHuisnr ?? ""} {(pa.OpdrachtPlaats != null ? $"({pa.OpdrachtPlaats})" : "")}".Trim();
                                break;
                            case "kiwanumber":
                                value = pa.Customer?.KiwaNummer;
                                break;
                            default:
                                var property = typeof(ProjectAssignment).GetProperty(field.FieldName);
                                value = property?.GetValue(pa);
                                break;
                        }

                        dict[field.FieldName] = value ?? DBNull.Value;

                        if (!string.IsNullOrEmpty(field.BackgroundColorRule))
                        {
                            var color = EvaluateBackgroundColor(field.BackgroundColorRule, pa, field.FieldName);
                            if (!string.IsNullOrEmpty(color))
                                dict[$"{field.FieldName}_BackgroundColor"] = color;
                        }
                    }
                    return dict;
                }).ToList();

                return Results.Ok(new
                {
                    totalCount,
                    pageNumber,
                    pageSize,
                    listName,
                    data = result,
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

                    var queryStart = stopwatch.ElapsedMilliseconds;
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