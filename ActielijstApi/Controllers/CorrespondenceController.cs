using ActielijstApi.Dtos;
using ActielijstApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ActielijstApi.Data;
using Microsoft.EntityFrameworkCore;

namespace ActielijstApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CorrespondenceController : ControllerBase
    {
        private readonly CorrespondenceService _correspondenceService;
        private readonly IEmailService _emailService;
        private readonly ILogger<CorrespondenceController> _logger;
        private readonly ApplicationDbContext _context;

        public CorrespondenceController(
            CorrespondenceService correspondenceService,
            IEmailService emailService,
            ILogger<CorrespondenceController> logger,
            ApplicationDbContext context)
        {
            _correspondenceService = correspondenceService;
            _emailService = emailService;
            _logger = logger;
            _context = context;
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
                await _emailService.SendEmailForCorrespondenceAsync(request.CorrespondentieId, request.EmailAan);
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

        /// <summary>
        /// Retourneert een document als downloadbaar bestand op basis van het opgegeven bestandspad.
        /// </summary>
        /// <param name="request">Het verzoek met het bestandspad van het document.</param>
        /// <returns>Het document als een downloadbaar bestand.</returns>
        /// <response code="200">Document succesvol geretourneerd.</response>
        /// <response code="400">Ongeldig bestandspad opgegeven.</response>
        /// <response code="404">Document niet gevonden.</response>
        /// <response code="500">Fout bij het verwerken van het document.</response>
        [HttpPost("open")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(object))]
        public async Task<IActionResult> OpenDocument([FromBody] OpenDocumentRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.DocumentPath))
            {
                return BadRequest(new { error = "Document path is required." });
            }

            string basePath = (await _context.Globals.FirstOrDefaultAsync())?.SavePath ?? "C:\\Documents\\KlantBase\\";
            if (!request.DocumentPath.StartsWith(basePath, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest(new { error = "Invalid document path." });
            }

            if (!System.IO.File.Exists(request.DocumentPath)) // Expliciet System.IO.File gebruiken
            {
                _logger.LogError($"Document niet gevonden: {request.DocumentPath}");
                return NotFound(new { error = "Document not found." });
            }

            try
            {
                var fileStream = new System.IO.FileStream(request.DocumentPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                return new FileStreamResult(fileStream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                {
                    FileDownloadName = Path.GetFileName(request.DocumentPath)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Fout bij openen van document: {request.DocumentPath}.");
                return StatusCode(500, new { error = "Kon het document niet verwerken.", details = ex.Message });
            }
        }

        /// <summary>
        /// Retourneert een document als downloadbaar bestand op basis van het opgegeven correspondentie-ID.
        /// </summary>
        /// <param name="request">Het verzoek met het correspondentie-ID.</param>
        /// <returns>Het document als een downloadbaar bestand.</returns>
        /// <response code="200">Document succesvol geretourneerd.</response>
        /// <response code="400">Ongeldig correspondentie-ID opgegeven.</response>
        /// <response code="404">Correspondentie of document niet gevonden.</response>
        /// <response code="500">Fout bij het verwerken van het document.</response>
        [HttpPost("open/by-correspondence")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(object))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(object))]
        public async Task<IActionResult> OpenDocumentByCorrespondence([FromBody] OpenDocumentByCorrespondenceRequest request)
        {
            if (request.CorrespondentieId <= 0)
            {
                return BadRequest(new { error = "Invalid correspondence ID." });
            }

            var correspondentie = await _context.Correspondentie
                .FirstOrDefaultAsync(c => c.Id == request.CorrespondentieId);

            if (correspondentie == null)
            {
                return NotFound(new { error = "Correspondence not found." });
            }

            if (string.IsNullOrWhiteSpace(correspondentie.fldCorBestand))
            {
                return NotFound(new { error = "No document associated with this correspondence." });
            }

            string documentPath = correspondentie.fldCorBestand;
            string basePath = (await _context.Globals.FirstOrDefaultAsync())?.SavePath ?? "C:\\Documents\\KlantBase\\";
            //if (!documentPath.StartsWith(basePath, StringComparison.OrdinalIgnoreCase))
            //{
            //    return BadRequest(new { error = "Invalid document path." });
            //}

            if (!System.IO.File.Exists(documentPath)) // Expliciet System.IO.File gebruiken
            {
                _logger.LogError($"Document niet gevonden: {documentPath}");
                return NotFound(new { error = "Document not found." });
            }

            try
            {
                var fileStream = new System.IO.FileStream(documentPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                return new FileStreamResult(fileStream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document")
                {
                    FileDownloadName = Path.GetFileName(documentPath)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Fout bij openen van document voor Correspondentie ID: {request.CorrespondentieId}.");
                return StatusCode(500, new { error = "Kon het document niet verwerken.", details = ex.Message });
            }
        }
    }

    public class OpenDocumentRequest
    {
        public string DocumentPath { get; set; }
    }

    public class OpenDocumentByCorrespondenceRequest
    {
        public int CorrespondentieId { get; set; }
    }
}