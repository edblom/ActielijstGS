using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ActielijstApi.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

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

// Login Endpoint (al aanwezig)
app.MapPost("/api/login", async (LoginRequest login, ApplicationDbContext context) =>
{
    var werknemer = await context.Werknemers
        .FirstOrDefaultAsync(w => w.Voornaam == login.Voornaam && w.FldLoginNaam == login.FldLoginNaam);

    if (werknemer == null)
    {
        return Results.Unauthorized();
    }

    return Results.Ok(new { WerknId = werknemer.WerknId, Voornaam = werknemer.Voornaam });
})
.WithName("Login")
.WithOpenApi();

// Nieuwe Werknemers Endpoint voor Dropdown
app.MapGet("/api/werknemers", async (ApplicationDbContext context) =>
{
    var workers = await context.Werknemers.ToListAsync();
    return Results.Ok(workers);
})
.WithName("GetWerknemers")
.WithOpenApi();

// Bestaande Memo Endpoints
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
    existingMemo.FldMActieDatum = updatedMemo.FldMActieDatum;
    existingMemo.FldMActieSoort = updatedMemo.FldMActieSoort;
    existingMemo.WerknId = updatedMemo.WerknId;

    try
    {
        await context.SaveChangesAsync();
        return Results.NoContent();
    }
    catch (DbUpdateConcurrencyException ex)
    {
        return Results.Conflict(new { error = "Concurrency conflict: The record was modified by another user." });
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

app.MapGet("/api/memos/test-error", () =>
{
    throw new ArgumentException("Dit is een testfout!");
})
.WithName("TestError")
.WithOpenApi();

// Nieuwe endpoint voor prioriteiten
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

app.UseExceptionMiddleware();

app.Run();

public class LoginRequest
{
    public string Voornaam { get; set; }
    public string FldLoginNaam { get; set; }
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
            _logger.LogError(ex, "Een onverwerkte fout is opgetreden.");
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