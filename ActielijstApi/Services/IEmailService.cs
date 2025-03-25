using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActielijstApi.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailRequest request);
    }

    public class EmailRequest
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public List<string> Attachments { get; set; } = new List<string>();
    }
}