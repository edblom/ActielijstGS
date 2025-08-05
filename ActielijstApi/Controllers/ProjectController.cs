using KlantBaseShare.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

[Route("api/projecten")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly ProjectService _projectService;

    public ProjectController(ProjectService projectService)
    {
        _projectService = projectService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectDTO>> Get(int id)
    {
        var project = await _projectService.GetByIdAsync(id);
        return Ok(project);
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<ProjectDTO>>> Search(string term, int limit = 50)
    {
        var results = await _projectService.SearchAsync(term, limit);
        return Ok(results);
    }
}
