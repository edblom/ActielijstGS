using ActielijstApi.Dtos;
using ActielijstApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ActieLijstAPI.Controllers
{
    [Route("api/contactpersonen")]
    [ApiController]
    public class ContactpersoonController : ControllerBase
    {
        private readonly ContactpersoonService _contactpersoonService;

        public ContactpersoonController(ContactpersoonService contactpersoonService)
        {
            _contactpersoonService = contactpersoonService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ContactpersoonDTO>> Get(int id)
        {
            var contact = await _contactpersoonService.GetByIdAsync(id);
            return Ok(contact);
        }
    }
}