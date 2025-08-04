using ActielijstApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[Route("api/opdrachten")]
[ApiController]
public class OpdrachtController : ControllerBase
{
    private readonly OpdrachtService _opdrachtService;

    public OpdrachtController(OpdrachtService opdrachtService)
    {
        _opdrachtService = opdrachtService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OpdrachtDTO>> Get(int id)
    {
        var opdracht = await _opdrachtService.GetByIdAsync(id);
        return Ok(opdracht);
    }
}