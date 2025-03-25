using ActielijstApi.Data;
using ActielijstApi.Dtos;
using ActielijstApi.Helpers;
using ActielijstApi.Models;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ActielijstApi.Services
{
    public class CorrespondenceService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CorrespondenceService> _logger;
        private readonly IEmailService _emailService;
        private readonly GlobalsService _globalsService;

        public CorrespondenceService(
            ApplicationDbContext context,
            ILogger<CorrespondenceService> logger,
            IEmailService emailService,
            GlobalsService globalsService)
        {
            _context = context;
            _logger = logger;
            _emailService = emailService;
            _globalsService = globalsService;
        }

        public async Task<CorrespondenceResponse> GenerateCorrespondenceAsync(GenerateCorrespondenceRequest request)
        {
            // 1. Haal het juiste sjabloon op basis van Soort
            var standaardDoc = await _context.StandaardDocs
                .FirstOrDefaultAsync(sd => sd.Soort == request.Soort)
                ?? throw new Exception($"Geen sjabloon gevonden voor soort {request.Soort}");

            // 2. Maak een Correspondentie-record
            var correspondentie = new Correspondentie
            {
                KlantID = request.KlantId,
                fldCorProjNum = request.ProjectId,
                fldCorOpdrachtNum = request.OpdrachtId,
                fldCorAuteur = "SystemUser", // Later: haal de ingelogde gebruiker op
                fldCorDatum = DateTime.Now,
                fldCorOmschrijving = request.Omschrijving ?? standaardDoc.DocOmschrijving,
                fldCorCPersId = request.ContactpersoonId
            };
            _context.Correspondentie.Add(correspondentie);
            await _context.SaveChangesAsync();

            // 3. Genereer het document
            string documentPath = await GenerateDocumentAsync(standaardDoc, correspondentie);

            // 4. Werk het bestandspad bij in Correspondentie
            correspondentie.fldCorBestand = documentPath;
            await _context.SaveChangesAsync();

            // 5. Optioneel: open het document in Word
            if (request.OpenDocumentInWord)
            {
                OpenDocumentInWord(documentPath);
            }

            // 6. Optioneel: verzend e-mail
            bool emailSent = false;
            if (request.SendEmail && !string.IsNullOrEmpty(standaardDoc.EmailSjabloon))
            {
                try
                {
                    await _emailService.SendEmailAsync(new EmailRequest
                    {
                        To = standaardDoc.EmailAan ?? _globalsService.GetFactuurAccount(),
                        Subject = standaardDoc.EmailSubject ?? "Document",
                        Body = standaardDoc.EmailSjabloon,
                        Attachments = new List<string> { documentPath }
                    });
                    emailSent = true;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Fout bij verzenden van e-mail");
                    emailSent = false;
                }
            }

            return new CorrespondenceResponse
            {
                CorrespondentieId = correspondentie.Id,
                FilePath = documentPath,
                EmailSent = emailSent
            };
        }

        private async Task<string> GenerateDocumentAsync(StandaardDoc standaardDoc, Correspondentie correspondentie)
        {
            var fields = await _context.CorrespondentieFields
                .FirstOrDefaultAsync(cf => cf.CorrespondentieNr == correspondentie.Id);

            // Haal paden op uit GlobalsService
            string defaultTemplatePath = _globalsService.GetSjabloonPath();
            string defaultSavePath = _globalsService.GetSavePath();
            string defaultProjectPath = _globalsService.GetProjectPath();
            string defaultDocPrefix = _globalsService.GetDefaultDocPrefix();

            // Bepaal het sjabloonpad op basis van PathDoc
            string templatePath;
            if (!string.IsNullOrWhiteSpace(standaardDoc.PathDoc))
            {
                // Controleer of PathDoc een volledig pad is
                if (Path.IsPathFullyQualified(standaardDoc.PathDoc))
                {
                    // PathDoc is een volledig pad (bijv. "M:\projectdossier\sjablonen\voorstel.docx")
                    templatePath = standaardDoc.PathDoc;
                    _logger.LogInformation($"Gebruik volledig pad uit StandaardDoc.PathDoc: {templatePath}");
                }
                else
                {
                    // PathDoc is alleen een bestandsnaam (bijv. "voorstel.docx")
                    // Combineer met defaultTemplatePath
                    string templateName = standaardDoc.PathDoc;

                    // Controleer of de sjabloonnaam een toegestane extensie heeft
                    var allowedExtensions = new[] { ".docx", ".dotx" };
                    if (!allowedExtensions.Any(ext => templateName.EndsWith(ext, StringComparison.OrdinalIgnoreCase)))
                    {
                        templateName = $"{templateName}.docx";
                        _logger.LogInformation($"Sjabloonnaam aangevuld met .docx-extensie: {templateName}");
                    }

                    // Maak de bestandsnaam veilig door ongeldige tekens te verwijderen
                    templateName = string.Join("_", templateName.Split(Path.GetInvalidFileNameChars()));

                    templatePath = Path.Combine(defaultTemplatePath, templateName);
                    _logger.LogInformation($"Sjabloonpad samengesteld uit defaultTemplatePath en PathDoc: {templatePath}");
                }
            }
            else
            {
                // Als PathDoc leeg is, gebruik een standaardsjabloon
                templatePath = Path.Combine(defaultTemplatePath, "voorstel.docx");
                _logger.LogWarning($"Geen sjabloonpad opgegeven in StandaardDoc.PathDoc, gebruik standaardpad: {templatePath}");
            }

            // Bepaal het opslagpad
            string savePath;
            if (standaardDoc.ProjectMap == true) // Nu een bool, direct controleren
            {
                // Gebruik de projectmap als ProjectMap is ingesteld op "true"
                if (!correspondentie.fldCorProjNum.HasValue)
                    throw new Exception("ProjectMap is ingesteld, maar fldCorProjNum is niet opgegeven in Correspondentie.");

                savePath = CalculateProjectPath(correspondentie.fldCorProjNum.Value);
                _logger.LogInformation($"Gebruik berekende projectmap: {savePath}");
            }
            else
            {
                // Gebruik de standaard opslaglocatie
                savePath = standaardDoc.DocSavePath ?? defaultSavePath;
                _logger.LogInformation($"Gebruik standaard opslagpad: {savePath}");
            }

            string docPrefix = standaardDoc.DocPrefix ?? defaultDocPrefix;
            string documentPath = Path.Combine(savePath, $"{docPrefix}_{correspondentie.Id}.docx");

            string directory = Path.GetDirectoryName(documentPath);
            if (directory != null && !Directory.Exists(directory))
            {
                try
                {
                    Directory.CreateDirectory(directory);
                    _logger.LogInformation($"Map aangemaakt: {directory}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Fout bij aanmaken van map: {directory}");
                    throw new Exception($"Kon de map niet aanmaken: {directory}", ex);
                }
            }

            if (!File.Exists(templatePath))
            {
                _logger.LogError($"Sjabloon niet gevonden: {templatePath}");
                throw new FileNotFoundException($"Sjabloon niet gevonden: {templatePath}");
            }

            try
            {
                File.Copy(templatePath, documentPath, true);
                _logger.LogInformation($"Sjabloon gekopieerd van {templatePath} naar {documentPath}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Fout bij kopiëren van sjabloon van {templatePath} naar {documentPath}");
                throw new Exception($"Kon het sjabloon niet kopiëren naar {documentPath}", ex);
            }

            using (WordprocessingDocument doc = WordprocessingDocument.Open(documentPath, true))
            {
                var customPropsPart = doc.CustomFilePropertiesPart ?? doc.AddCustomFilePropertiesPart();
                customPropsPart.Properties ??= new DocumentFormat.OpenXml.CustomProperties.Properties();

                if (fields != null)
                {
                    foreach (var prop in customPropsPart.Properties.Elements<DocumentFormat.OpenXml.CustomProperties.CustomDocumentProperty>())
                    {
                        string? propName = prop.Name?.Value;
                        if (string.IsNullOrEmpty(propName)) continue;

                        string? fieldValue = typeof(CorrespondentieFields).GetProperty(propName)?.GetValue(fields)?.ToString();
                        if (fieldValue != null)
                        {
                            DocumentHelper.UpdatePropertyValue(prop, fieldValue);
                            _logger.LogInformation($"Property '{propName}' gewijzigd naar: '{fieldValue}'");
                        }
                    }
                }
                customPropsPart.Properties.Save();
                DocumentHelper.UpdateFieldsInDocument(doc);
                doc.MainDocumentPart?.Document.Save();
            }

            return documentPath;
        }

        private void OpenDocumentInWord(string documentPath)
        {
            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = documentPath,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fout bij openen van document in Word");
                throw new Exception("Kon het document niet openen in Word.", ex);
            }
        }

        // Helperfunctie om ongeldige tekens te vervangen (equivalent van VBA's validName)
        private string ValidName(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Vervang ongeldige tekens door een underscore
            return string.Join("_", input.Split(Path.GetInvalidFileNameChars()));
        }

        // Bereken het projectpad op basis van projNum
        private string CalculateProjectPath(int projNum)
        {
            // Haal pdfPath op uit GlobalsService
            string pathStr = _globalsService.GetPdfPath();
            if (string.IsNullOrWhiteSpace(pathStr))
                throw new Exception("pdfPath in tblGlobals is leeg.");

            if (!pathStr.EndsWith("\\"))
                pathStr += "\\";

            // Haal projectgegevens op uit tblprojecten
            var project = _context.Projecten
                .FirstOrDefault(p => p.Id == projNum)
                ?? throw new Exception($"Project met ID {projNum} niet gevonden in tblprojecten.");

            // Valideer verplichte velden
            if (string.IsNullOrWhiteSpace(project.FldJaar))
                throw new Exception($"fldJaar is leeg voor project {projNum}.");
            if (!project.FldProjectNummer.HasValue)
                throw new Exception($"fldProjectNummer is leeg voor project {projNum}.");

            string varJaar = project.FldJaar;
            string varProjectnummer = project.FldProjectNummer.Value.ToString();
            string varNum = project.FldExternNummer;

            // Als fldExternNummer leeg is, bereken varNum
            if (string.IsNullOrEmpty(varNum))
            {
                varNum = $"{project.FldProjectNummer.Value:D3}GS{varJaar}"; // Equivalent van Format(CInt(varProjectnummer), "000") & "GS" & varJaar
            }

            string varDesc = project.FldProjectNaam ?? string.Empty;
            string varPlaats = project.FldPlaats ?? string.Empty;
            string varAdres = project.FldAdres ?? string.Empty;

            // Combineer alles tot het projectpad
            pathStr = pathStr + varJaar + "\\" + varNum + "_" + ValidName(varPlaats) + "_" + ValidName(varDesc) + "_" + ValidName(varAdres);
            return pathStr;
        }
    }
}