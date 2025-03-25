using ActielijstApi.Data;
using ActielijstApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace ActielijstApi.Services
{
    public class GlobalsService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<GlobalsService> _logger;
        private Globals _globals;

        public GlobalsService(ApplicationDbContext context, ILogger<GlobalsService> logger)
        {
            _context = context;
            _logger = logger;
            LoadGlobals();
        }

        private void LoadGlobals()
        {
            try
            {
                // Haal de eerste (en enige) rij op uit tblGlobals
                _globals = _context.Globals.FirstOrDefault()
                    ?? throw new InvalidOperationException("Geen instellingen gevonden in tblGlobals. Zorg ervoor dat er een rij bestaat.");

                _logger.LogInformation("Instellingen geladen uit tblGlobals.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij laden van instellingen uit tblGlobals.");
                throw;
            }
        }

        // Methodes om specifieke instellingen op te halen
        public string GetSjabloonPath() => _globals.SjabloonPath ?? "M:\\projectdossier\\sjablonen\\";
        public string GetSavePath() => _globals.SavePath ?? "M:\\projectdossier\\2025\\documenten\\";
        public string GetProjectPath() => _globals.ProjectPath ?? "M:\\projectdossier\\2025\\";
        public string GetDefaultDocPrefix() => "doc"; // Geen specifieke kolom in tblGlobals, gebruik een standaardwaarde
        public string GetFactuurText() => _globals.FactuurText ?? "Standaard factuurtekst";
        public string GetFactuurAccount() => _globals.FactuurAccount ?? "factuur@voorbeeld.nl";
        public bool GetDisplayMailVoorVerzenden() => _globals.DisplayMailVoorVerzenden;
        public string GetPdfPath() => _globals.PdfPath ?? "M:\\Projectdossier\\"; // Nieuwe methode voor pdfPath
    }
}