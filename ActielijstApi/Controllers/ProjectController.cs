using ActielijstApi.Dtos;
using Microsoft.AspNetCore.Mvc;
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
}