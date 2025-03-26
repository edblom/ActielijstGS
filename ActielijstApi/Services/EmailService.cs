using ActielijstApi.Data;
using ActielijstApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System;
using Microsoft.Extensions.Configuration; // Toevoegen voor IConfiguration

namespace ActielijstApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly ApplicationDbContext _context;
        private readonly GlobalsService _globalsService;
        private readonly ILogger<EmailService> _logger;
        private readonly SmtpSettings _smtpSettings;
        private readonly IConfiguration _configuration; // Toevoegen voor toegang tot configuratie

        public EmailService(
            ApplicationDbContext context,
            GlobalsService globalsService,
            ILogger<EmailService> logger,
            IOptions<SmtpSettings> smtpSettings,
            IConfiguration configuration) // Injecteer IConfiguration
        {
            _context = context;
            _globalsService = globalsService;
            _logger = logger;
            _smtpSettings = smtpSettings.Value;
            _configuration = configuration;
        }

        public async Task SendEmailAsync(EmailRequest request)
        {
            // Valideer de invoer
            if (string.IsNullOrWhiteSpace(request.To))
                throw new ArgumentException("Geen e-mailadres opgegeven voor ontvanger (To).");
            if (string.IsNullOrWhiteSpace(request.Subject))
                throw new ArgumentException("Geen onderwerp opgegeven (Subject).");
            if (string.IsNullOrWhiteSpace(request.Body))
                throw new ArgumentException("Geen e-mailbody opgegeven (Body).");

            // Haal de instellingen uit tblGlobals
            string fromEmail = _globalsService.GetFactuurAccount() ?? _smtpSettings.FromEmail;
            bool displayMailBeforeSending = _globalsService.GetDisplayMailVoorVerzenden();

            // Haal het wachtwoord uit de configuratie (user-secrets of omgevingsvariabelen)
            string password = _configuration["EmailSettings:Password"];
            if (string.IsNullOrEmpty(password))
            {
                _logger.LogError("E-mailwachtwoord is niet ingesteld in de configuratie.");
                throw new Exception("E-mailwachtwoord is niet ingesteld. Configureer 'EmailSettings:Password' in user-secrets of omgevingsvariabelen.");
            }

            // Maak de e-mail
            using var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail, _smtpSettings.FromName),
                Subject = request.Subject,
                Body = request.Body,
                IsBodyHtml = false // Stel op true als je HTML wilt gebruiken
            };
            mailMessage.To.Add(request.To);

            // Voeg bijlagen toe
            foreach (var attachmentPath in request.Attachments)
            {
                if (!File.Exists(attachmentPath))
                    throw new FileNotFoundException($"Bijlage niet gevonden: {attachmentPath}");
                mailMessage.Attachments.Add(new Attachment(attachmentPath));
            }

            // Configureer de SMTP-client
            using var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
            {
                EnableSsl = _smtpSettings.EnableSsl,
                Credentials = new NetworkCredential(_smtpSettings.Username, password) // Gebruik het dynamisch opgehaalde wachtwoord
            };

            try
            {
                // Als DisplayMailVoorVerzenden true is, loggen we de e-maildetails
                if (displayMailBeforeSending)
                {
                    _logger.LogInformation($"E-mail preview: Van: {fromEmail}, Naar: {request.To}, Onderwerp: {request.Subject}, Body: {request.Body}, Bijlagen: {string.Join(", ", request.Attachments)}");
                    // In een API-context kunnen we geen preview tonen, maar dit kan worden uitgebreid met een preview-endpoint
                }

                // Verstuur de e-mail
                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation($"E-mail verzonden naar {request.To}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Fout bij verzenden van e-mail naar {request.To}.");
                throw new Exception("Kon de e-mail niet verzenden.", ex);
            }
        }

        public async Task SendEmailForCorrespondenceAsync(int correspondentieId)
        {
            // Haal de Correspondentie-record op
            var correspondentie = await _context.Correspondentie
                .FirstOrDefaultAsync(c => c.Id == correspondentieId)
                ?? throw new Exception($"Correspondentie met ID {correspondentieId} niet gevonden.");

            // Haal de bijbehorende StandaardDoc op
            var standaardDoc = await _context.StandaardDocs
                .FirstOrDefaultAsync(sd => sd.Soort == correspondentie.fldCorSoort)
                ?? throw new Exception($"Geen sjabloon gevonden voor soort {correspondentie.fldCorSoort}.");

            // Haal e-maildetails uit StandaardDoc
            string toEmail = standaardDoc.EmailAan ?? throw new Exception("Geen e-mailadres opgegeven in StandaardDoc (EmailAan).");
            string subject = standaardDoc.EmailSubject ?? "Geen onderwerp opgegeven";
            string body = standaardDoc.EmailSjabloon ?? "Geen e-mailbody opgegeven";

            // Haal het pad van het gegenereerde document
            string documentPath = correspondentie.fldCorBestand ?? throw new Exception("Geen documentpad gevonden in Correspondentie (fldCorBestand).");

            // Maak een EmailRequest
            var emailRequest = new EmailRequest
            {
                To = toEmail,
                Subject = subject,
                Body = body,
                Attachments = new List<string> { documentPath }
            };

            // Verstuur de e-mail
            await SendEmailAsync(emailRequest);

            // Update de Correspondentie-record met de verzenddatum
            correspondentie.fldCorDatum2 = DateTime.Now; // Stel de huidige datum/tijd in
            await _context.SaveChangesAsync();
        }
    }
}