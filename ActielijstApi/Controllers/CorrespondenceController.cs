using ActielijstApi.Dtos;
using ActielijstApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;

namespace ActielijstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorrespondenceController : ControllerBase
    {
        private readonly CorrespondenceService _correspondenceService;
        private readonly IEmailService _emailService;
        private readonly ILogger<CorrespondenceController> _logger;

        public CorrespondenceController(
            CorrespondenceService correspondenceService,
            IEmailService emailService,
            ILogger<CorrespondenceController> logger)
        {
            _correspondenceService = correspondenceService;
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost("generate")]
        public async Task<ActionResult<GenerateCorrespondenceResponse>> Generate([FromBody] GenerateCorrespondenceRequest request)
        {
            try
            {
                var response = await _correspondenceService.GenerateCorrespondenceAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Fout bij genereren van correspondentie voor Soort {request.Soort}.");
                return StatusCode(500, new { error = "Kon de correspondentie niet genereren.", details = ex.Message });
            }
        }

        [HttpPost("send-email")]
        public async Task<ActionResult<SendEmailResponse>> SendEmail([FromBody] SendEmailRequest request)
        {
            try
            {
                await _emailService.SendEmailForCorrespondenceAsync(request.CorrespondentieId);
                return Ok(new SendEmailResponse
                {
                    CorrespondentieId = request.CorrespondentieId,
                    EmailSent = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Fout bij verzenden van e-mail voor Correspondentie ID {request.CorrespondentieId}.");
                return StatusCode(500, new { error = "Kon de e-mail niet verzenden.", details = ex.Message });
            }
        }
    }
}