using Kipu.API.Projects.Application.Services;
using Kipu.API.Projects.Domain.Model.Commands;
using Kipu.API.Projects.Domain.Model.Queries;
using Kipu.API.Projects.Interfaces.REST.Resources;
using Kipu.API.Projects.Interfaces.REST.Transform;
using Kipu.API.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Kipu.API.Projects.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class ProjectsController(
    IProjectCommandService projectCommandService,
    IProjectQueryService projectQueryService,
    IStringLocalizer<SharedResource> localizer) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectResource resource)
    {
        var command = CreateProjectCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await projectCommandService.Handle(command);

        return result.Fold<IActionResult>(
            project => CreatedAtAction(nameof(GetProjectById), new { id = project.Id }, ProjectResourceFromEntityAssembler.ToResourceFromEntity(project)),
            error => BadRequest(new { message = localizer[error].Value })
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProjects()
    {
        var query = new GetAllProjectsQuery();
        var projects = await projectQueryService.Handle(query);
        var resources = projects.Select(ProjectResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProjectById(int id)
    {
        var query = new GetProjectByIdQuery(id);
        var project = await projectQueryService.Handle(query);

        if (project == null)
        {
            return NotFound(new { message = string.Format(localizer["ProjectNotFoundWithId"].Value, id) });
        }

        return Ok(ProjectResourceFromEntityAssembler.ToResourceFromEntity(project));
    }

    [HttpPatch("{id:int}")]
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateProjectStatus(int id, [FromBody] UpdateProjectStatusResource resource)
    {
        var command = UpdateProjectStatusCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await projectCommandService.Handle(command);

        return result.Fold<IActionResult>(
            project => Ok(ProjectResourceFromEntityAssembler.ToResourceFromEntity(project)),
            error => BadRequest(new { message = localizer[error].Value })
        );
    }

    [HttpPost("{id:int}/items")]
    public async Task<IActionResult> AddProjectItems(int id, [FromBody] List<CreateProjectItemResource> resources)
    {
        var requests = resources.Select(r => new ProjectItemRequest(r.Name, r.StartDate, r.EndDate)).ToList();
        var command = new AddProjectItemsCommand(id, requests);
        var result = await projectCommandService.Handle(command);

        return result.Fold<IActionResult>(
            items =>
            {
                var itemResources = items.Select(ProjectItemResourceFromEntityAssembler.ToResourceFromEntity);
                return StatusCode(201, itemResources);
            },
            error => BadRequest(new { message = localizer[error].Value })
        );
    }

    [HttpGet("{id:int}/items")]
    public async Task<IActionResult> GetProjectItems(int id)
    {
        var query = new GetProjectItemsQuery(id);
        var items = await projectQueryService.Handle(query);
        var resources = items.Select(ProjectItemResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var command = new DeleteProjectCommand(id);
        var result = await projectCommandService.Handle(command);

        return result.Fold<IActionResult>(
            project => Ok(new { message = "Project deleted successfully" }),
            error => BadRequest(new { message = localizer[error].Value })
        );
    }
}
