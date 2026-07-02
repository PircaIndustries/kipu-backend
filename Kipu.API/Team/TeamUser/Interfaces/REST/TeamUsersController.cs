using System.Net.Mime;
using System.Text.Json;
using Kipu.API.Notifications.Application.Services;
using Kipu.API.Notifications.Domain.Model.Commands;
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
    private readonly INotificationCommandService _notificationCommandService;
    private readonly IStringLocalizer<SharedResource> _localizer;

    public TeamUsersController(
        ITeamUserCommandService commandService,
        ITeamUserQueryService queryService,
        INotificationCommandService notificationCommandService,
        IStringLocalizer<SharedResource> localizer)
    {
        _commandService = commandService;
        _queryService = queryService;
        _notificationCommandService = notificationCommandService;
        _localizer = localizer;
    }
    
    [HttpPost]
    public async Task<ActionResult> CreateTeamUser([FromBody] CreateTeamUserResource resource)
    {
        // Serialize resource into Payload
        var payload = JsonSerializer.Serialize(resource);
        var message = $"Has sido invitado a unirte al proyecto con el rol de {resource.Role}";
        var notificationCommand = new CreateNotificationCommand(resource.UserId.ToString(), message, "ProjectInvitation", resource.ProjectId, payload);
        
        var notificationResult = await _notificationCommandService.Handle(notificationCommand);
        
        if (notificationResult == null)
            return BadRequest(new { message = "No se pudo enviar la invitación." });
        
        return Ok(new { message = "Invitación enviada exitosamente." });
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
