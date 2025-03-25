using System;
using System.Threading.Tasks;
namespace ActielijstApi.Services
{
    public class EmailService : IEmailService
    {
        public Task SendEmailAsync(EmailRequest request)
        {
            // Placeholder: later implementeren met MailKit of een andere e-mailclient
            throw new NotImplementedException("E-mailfunctionaliteit is nog niet geïmplementeerd.");
        }
    }
}