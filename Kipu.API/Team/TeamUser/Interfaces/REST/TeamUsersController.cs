using System.Net.Mime;
using Kipu.API.Resources;
using Kipu.API.Team.TeamUser.application.Services;
using Kipu.API.Team.TeamUser.domain.model.Commands;
using Kipu.API.Team.TeamUser.domain.model.Queries;
using Kipu.API.Team.TeamUser.Interfaces.REST.Resources;
using Kipu.API.Team.TeamUser.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace Kipu.API.Team.TeamUser.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class TeamUsersController : ControllerBase
{
    private readonly ITeamUserCommandService _commandService;
    private readonly ITeamUserQueryService _queryService;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public TeamUsersController(
        ITeamUserCommandService commandService,
        ITeamUserQueryService queryService,
        IStringLocalizer<SharedResource> localizer)
    {
        _commandService = commandService;
        _queryService = queryService;
        _localizer = localizer;
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateTeamUser([FromBody] CreateTeamUserResource resource)
    {
        var command = CreateTeamUserCommandFromResourceAssembler.ToCommandFromResource(resource);
        
        var result = await _commandService.Handle(command);
        
        if (result is null)
            return BadRequest(new { message = _localizer["TeamUserNotCreated"].Value });
        
        var userResource = TeamUserResourceFromEntityAssembler.ToResourceFromEntity(result);
        
        return CreatedAtAction(nameof(GetTeamUserById), new { id = userResource.Id }, userResource);
    }
    
    [HttpPost("{id}/activate")]
    public async Task<ActionResult> ActivateTeamUser(string id)
    {
        var command = new ActivateTeamUserCommand(id);
        var result = await _commandService.Handle(command);
        
        if (result is null)
            return NotFound(new { message = _localizer["TeamUserNotFound"].Value });
        
        var resource = TeamUserResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }

    [HttpPost("{id}/deactivate")]
    public async Task<ActionResult> DeactivateTeamUser(string id)
    {
        var command = new DeactivateTeamUserCommand(id);
        var result = await _commandService.Handle(command);
        
        if (result is null)
            return NotFound(new { message = _localizer["TeamUserNotFound"].Value });
        
        var resource = TeamUserResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }
    
    [HttpGet("{id}")]
    public async Task<ActionResult> GetTeamUserById(string id)
    {
        var query = new GetTeamUserByIdQuery(id);
        var result = await _queryService.Handle(query);
        
        if (result is null)
            return NotFound(new { message = _localizer["TeamUserNotFound"].Value });
        
        var resource = TeamUserResourceFromEntityAssembler.ToResourceFromEntity(result);
        return Ok(resource);
    }
    
    [HttpGet]
    public async Task<ActionResult> GetAllTeamUsers(
        [FromQuery] string projectId,
        [FromQuery] string? globalSearch, 
        [FromQuery] string? role, 
        [FromQuery] bool? isActive)
    {
        if (string.IsNullOrWhiteSpace(globalSearch) && string.IsNullOrWhiteSpace(role) && !isActive.HasValue)
        {
            var allUsers = await _queryService.Handle(new GetAllTeamUsersQuery(projectId));
            var resources = allUsers.Select(TeamUserResourceFromEntityAssembler.ToResourceFromEntity);
            return Ok(resources);
        }

        var query = new GetAllTeamUsersByFilterQuery(projectId, globalSearch, role, isActive);
        var filteredUsers = await _queryService.Handle(query);
        
        var filteredResources = filteredUsers.Select(TeamUserResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(filteredResources);
    }
}