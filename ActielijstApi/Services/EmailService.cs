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
using Microsoft.Extensions.Configuration;
using Azure.Core;

namespace ActielijstApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly ApplicationDbContext _context;
        private readonly GlobalsService _globalsService;
        private readonly ILogger<EmailService> _logger;
        private readonly SmtpSettings _smtpSettings;
        private readonly IConfiguration _configuration;

        public EmailService(
            ApplicationDbContext context,
            GlobalsService globalsService,
            ILogger<EmailService> logger,
            IOptions<SmtpSettings> smtpSettings,
            IConfiguration configuration)
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
            if (request.Attachments != null)
            {
                foreach (var attachmentPath in request.Attachments)
                {
                    if (!File.Exists(attachmentPath))
                        throw new FileNotFoundException($"Bijlage niet gevonden: {attachmentPath}");
                    mailMessage.Attachments.Add(new Attachment(attachmentPath));
                }
            }
            // Configureer de SMTP-client
            using var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port)
            {
                Port = _smtpSettings.Port,
                EnableSsl = _smtpSettings.EnableSsl,
                Credentials = new NetworkCredential(_smtpSettings.Username, password)
            };

            try
            {
                // Als DisplayMailVoorVerzenden true is, loggen we de e-maildetails
                if (displayMailBeforeSending)
                {
                    if (request.Attachments != null)
                    {
                        _logger.LogInformation($"E-mail preview: Van: {fromEmail}, Naar: {request.To}, Onderwerp: {request.Subject}, Body: {request.Body}, Bijlagen: {string.Join(", ", request.Attachments)}");
                    }
                    else
                    {
                        _logger.LogInformation($"E-mail preview: Van: {fromEmail}, Naar: {request.To}, Onderwerp: {request.Subject}, Body: {request.Body}");
                    }
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

        public async Task SendEmailForCorrespondenceAsync(int correspondentieId, string emailAan)
        {
            // Valideer de EmailAan-parameter
            if (string.IsNullOrWhiteSpace(emailAan))
                throw new ArgumentException("Geen e-mailadres opgegeven voor ontvanger (EmailAan).");

            // Haal de Correspondentie-record op
            var correspondentie = await _context.Correspondentie
                .FirstOrDefaultAsync(c => c.Id == correspondentieId)
                ?? throw new Exception($"Correspondentie met ID {correspondentieId} niet gevonden.");

            // Haal de bijbehorende StandaardDoc op
            var standaardDoc = await _context.StandaardDocs
                .FirstOrDefaultAsync(sd => sd.Soort == correspondentie.fldCorSoort)
                ?? throw new Exception($"Geen sjabloon gevonden voor soort {correspondentie.fldCorSoort}.");

            // Haal e-maildetails uit StandaardDoc (behalve EmailAan, dat nu uit de parameter komt)
            string subject = standaardDoc.EmailSubject ?? "Geen onderwerp opgegeven";
            string body = standaardDoc.EmailSjabloon ?? "Geen e-mailbody opgegeven";

            // Haal het pad van het gegenereerde document
            string documentPath = correspondentie.fldCorBestand ?? throw new Exception("Geen documentpad gevonden in Correspondentie (fldCorBestand).");

            // Maak een EmailRequest
            var emailRequest = new EmailRequest
            {
                To = emailAan, // Gebruik de meegegeven EmailAan-parameter
                Subject = subject,
                Body = body,
                Attachments = new List<string> { documentPath }
            };
            var testRequest = new EmailRequest
            {
                To = "ed@klantbase.nl",
                Subject = "Test",
                Body = "Hallo",
                Attachments = null
            };
            await SendEmailAsync(testRequest);

            // Log voor debuggen
            Console.WriteLine($"Versturen naar: {emailRequest.To}, Onderwerp: {emailRequest.Subject}");
            Console.WriteLine($"SMTP: Host={_smtpSettings.Host}, Port={_smtpSettings.Port}, SSL={_smtpSettings.EnableSsl}");
            Console.WriteLine($"Gebruikersnaam: {_smtpSettings.Username}");

            // Verstuur de e-mail
            await SendEmailAsync(emailRequest);

            // Update de Correspondentie-record met de verzenddatum
            correspondentie.fldCorDatum2 = DateTime.Now;
            await _context.SaveChangesAsync();
        }
    }
}