using ActielijstApi.Dtos;
using ActielijstApi.Services;
using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// Genereert een correspondentiedocument op basis van het verzoek.
        /// </summary>
        /// <param name="request">Het verzoek met details voor de correspondentie.</param>
        /// <returns>Een GenerateCorrespondenceResponse met de details van het gegenereerde document.</returns>
        /// <response code="200">Document succesvol gegenereerd.</response>
        /// <response code="500">Fout bij het genereren van het document.</response>
        [HttpPost("generate")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GenerateCorrespondenceResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(object))]
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

        /// <summary>
        /// Verstuurt een e-mail voor een specifieke correspondentie.
        /// </summary>
        /// <param name="request">Het verzoek met de CorrespondentieId.</param>
        /// <returns>Een SendEmailResponse met de status van de verzending.</returns>
        /// <response code="200">E-mail succesvol verzonden.</response>
        /// <response code="500">Fout bij het verzenden van de e-mail.</response>
        [HttpPost("send-email")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SendEmailResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(object))]
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