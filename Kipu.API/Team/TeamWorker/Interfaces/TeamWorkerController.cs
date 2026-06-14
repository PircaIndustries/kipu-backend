using Kipu.API.Team.TeamWorker.Application.Services;
using Kipu.API.Team.TeamWorker.Domain.Model.Commands;
using Kipu.API.Team.TeamWorker.Domain.Model.Queries;
using Kipu.API.Team.TeamWorker.Interfaces.Resources;
using Kipu.API.Team.TeamWorker.Interfaces.Transform;
using Microsoft.AspNetCore.Mvc;

namespace Kipu.API.Team.TeamWorker.Interfaces;

[ApiController]
[Route("api/v1/[controller]")]
public class TeamWorkersController : ControllerBase
{
    private readonly ITeamWorkerCommandService _teamWorkerCommandService;
    private readonly ITeamWorkerQueryService _teamWorkerQueryService;

    public TeamWorkersController(
        ITeamWorkerCommandService teamWorkerCommandService, 
        ITeamWorkerQueryService teamWorkerQueryService)
    {
        _teamWorkerCommandService = teamWorkerCommandService;
        _teamWorkerQueryService = teamWorkerQueryService;
    }


    [HttpPost]
    public async Task<IActionResult> CreateTeamWorker([FromBody] CreateTeamWorkerResource resource)
    {
        var command = CreateTeamWorkerCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await _teamWorkerCommandService.Handle(command);

        if (result is null) return BadRequest("No se pudo crear el trabajador.");

        var responseResource = TeamWorkerResourceFromEntityAssembler.ToResourceFromEntity(result);
        return CreatedAtAction(nameof(GetTeamWorkerById), new { teamWorkerId = result.Id.Value }, responseResource);
    }

    [HttpDelete("{teamWorkerId}")]
    public async Task<IActionResult> DeleteTeamWorker(string teamWorkerId)
    {
        var command = new DeleteTeamWorkerCommand(teamWorkerId);
        var success = await _teamWorkerCommandService.Handle(command);

        if (!success) return NotFound("El trabajador no existe o no se pudo eliminar.");

        return NoContent(); // 204 No Content es el estándar correcto para un DELETE exitoso
    }

    [HttpPost("{teamWorkerId}/machineries")]
    public async Task<IActionResult> AssignMachinery(string teamWorkerId, [FromBody] AssignMachineryResource resource)
    {
        var command = new AssignMachineryToTeamWorkerCommand(teamWorkerId, resource.MachineryId, resource.FullName);
        var result = await _teamWorkerCommandService.Handle(command);

        if (result is null) return BadRequest("Error al asignar la maquinaria.");

        var responseResource = TeamWorkerResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(responseResource);
    }

    [HttpDelete("{teamWorkerId}/machineries/{machineryId}")]
    public async Task<IActionResult> RemoveMachinery(string teamWorkerId, string machineryId)
    {
        var command = new RemoveMachineryFromTeamWorkerCommand(teamWorkerId, machineryId);
        var result = await _teamWorkerCommandService.Handle(command);

        if (result is null) return BadRequest("Error al remover la maquinaria.");

        var responseResource = TeamWorkerResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(responseResource);
    }


    [HttpGet("{teamWorkerId}")]
    public async Task<IActionResult> GetTeamWorkerById(string teamWorkerId)
    {
        var query = new GetTeamWorkerByIdQuery(teamWorkerId);
        var result = await _teamWorkerQueryService.Handle(query);

        if (result is null) return NotFound();

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