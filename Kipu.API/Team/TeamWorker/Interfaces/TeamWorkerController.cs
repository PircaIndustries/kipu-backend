using Kipu.API.Resources;
using Kipu.API.Team.TeamWorker.Application.Services;
using Kipu.API.Team.TeamWorker.Domain.Model.Commands;
using Kipu.API.Team.TeamWorker.Domain.Model.Queries;
using Kipu.API.Team.TeamWorker.Interfaces.Resources;
using Kipu.API.Team.TeamWorker.Interfaces.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Kipu.API.Team.TeamWorker.Interfaces;

[ApiController]
[Route("api/v1/[controller]")]
public class TeamWorkersController : ControllerBase
{
    private readonly ITeamWorkerCommandService _teamWorkerCommandService;
    private readonly ITeamWorkerQueryService _teamWorkerQueryService;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public TeamWorkersController(
        ITeamWorkerCommandService teamWorkerCommandService, 
        ITeamWorkerQueryService teamWorkerQueryService,
        IStringLocalizer<SharedResource> localizer)
    {
        _teamWorkerCommandService = teamWorkerCommandService;
        _teamWorkerQueryService = teamWorkerQueryService;
        _localizer = localizer;
    }


    [HttpPost]
    public async Task<IActionResult> CreateTeamWorker([FromBody] CreateTeamWorkerResource resource)
    {
        var command = CreateTeamWorkerCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await _teamWorkerCommandService.Handle(command);

        if (result is null)
            return BadRequest(new { message = _localizer["TeamWorkerNotCreated"].Value });

        var responseResource = TeamWorkerResourceFromEntityAssembler.ToResourceFromEntity(result);
        return CreatedAtAction(nameof(GetTeamWorkerById), new { teamWorkerId = result.Id.Value }, responseResource);
    }

    [HttpDelete("{teamWorkerId}")]
    public async Task<IActionResult> DeleteTeamWorker(string teamWorkerId)
    {
        var command = new DeleteTeamWorkerCommand(teamWorkerId);
        var success = await _teamWorkerCommandService.Handle(command);

        if (!success)
            return NotFound(new { message = _localizer["TeamWorkerNotDeleted"].Value });

        return NoContent();
    }

    [HttpPost("{teamWorkerId}/machineries")]
    public async Task<IActionResult> AssignMachinery(string teamWorkerId, [FromBody] AssignMachineryResource resource)
    {
        var command = new AssignMachineryToTeamWorkerCommand(teamWorkerId, resource.MachineryId, resource.FullName);
        var result = await _teamWorkerCommandService.Handle(command);

        if (result is null)
            return BadRequest(new { message = _localizer["MachineryAssignmentFailed"].Value });

        var responseResource = TeamWorkerResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(responseResource);
    }

    [HttpDelete("{teamWorkerId}/machineries/{machineryId}")]
    public async Task<IActionResult> RemoveMachinery(string teamWorkerId, string machineryId)
    {
        var command = new RemoveMachineryFromTeamWorkerCommand(teamWorkerId, machineryId);
        var result = await _teamWorkerCommandService.Handle(command);

        if (result is null)
            return BadRequest(new { message = _localizer["MachineryRemovalFailed"].Value });

        var responseResource = TeamWorkerResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(responseResource);
    }


    [HttpGet("{teamWorkerId}")]
    public async Task<IActionResult> GetTeamWorkerById(string teamWorkerId)
    {
        var query = new GetTeamWorkerByIdQuery(teamWorkerId);
        var result = await _teamWorkerQueryService.Handle(query);

        if (result is null)
            return NotFound(new { message = _localizer["TeamWorkerNotFound"].Value });

        var resource = TeamWorkerResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    [HttpGet]
    public async Task<IActionResult> GetTeamWorkers([FromQuery] string projectId, [FromQuery] string? globalSearch)
    {
        var query = new GetAllTeamWorkersByProjectIdQuery(projectId, globalSearch);
        var results = await _teamWorkerQueryService.Handle(query);

        var resources = results.Select(TeamWorkerResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
}