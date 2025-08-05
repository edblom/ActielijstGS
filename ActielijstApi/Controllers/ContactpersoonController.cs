using ActielijstApi.Dtos;
using ActielijstApi.Services;
using ActieLijstAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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


        [HttpGet("search")]
        public async Task<ActionResult<List<ContactpersoonDTO>>> Search(string term, int limit = 50)
        {
            var results = await _contactpersoonService.SearchAsync(term, limit);
            return Ok(results);
        }
    }
}