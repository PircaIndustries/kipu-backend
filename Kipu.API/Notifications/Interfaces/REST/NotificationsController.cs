using System.Text.Json;
using Kipu.API.Notifications.Application.Services;
using Kipu.API.Notifications.Domain.Model.Commands;
using Kipu.API.Notifications.Domain.Model.Queries;
using Kipu.API.Notifications.Interfaces.REST.Transform;
using Kipu.API.Team.TeamUser.application.Services;
using Kipu.API.Team.TeamUser.domain.model.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Kipu.API.Notifications.Interfaces.REST;

[ApiController]
[Route("api/v1/[controller]")]
public class NotificationsController(
    INotificationCommandService commandService,
    INotificationQueryService queryService,
    ITeamUserCommandService teamUserCommandService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetNotifications([FromQuery] string userId)
    {
        if (string.IsNullOrEmpty(userId)) return BadRequest(new { message = "UserId is required" });
        var notifications = await queryService.Handle(new GetNotificationsByUserIdQuery(userId));
        var resources = notifications.Select(NotificationResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpPost("{id:int}/accept")]
    public async Task<IActionResult> AcceptNotification(int id)
    {
        var result = await commandService.Handle(new AcceptNotificationCommand(id));
        if (result == null) return NotFound(new { message = "Notificación no encontrada" });
        
        if (result.Type == "ProjectInvitation" && !string.IsNullOrEmpty(result.Payload))
        {
            try 
            {
                var dict = JsonSerializer.Deserialize<Dictionary<string, object>>(result.Payload);
                if (dict != null)
                {
                    var userId = dict.ContainsKey("UserId") ? dict["UserId"].ToString() : result.UserId;
                    var fullName = dict.ContainsKey("FullName") ? dict["FullName"].ToString() : "";
                    var email = dict.ContainsKey("Email") ? dict["Email"].ToString() : "";
                    var role = dict.ContainsKey("Role") ? dict["Role"].ToString() : "Logistica";
                    var projectId = dict.ContainsKey("ProjectId") ? dict["ProjectId"].ToString() : result.RelatedProjectId;

                    var command = new CreateTeamUserCommand(int.Parse(userId), fullName, new Kipu.API.Team.TeamUser.domain.model.ValueObjects.Email(email), role, projectId);
                    var teamUserResult = await teamUserCommandService.Handle(command);
                    
                    if (teamUserResult != null)
                    {
                        teamUserResult.Activate();
                        // Assuming activation is managed separately or we just accept it created
                    }
                }
            } 
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing invitation payload: {ex.Message}");
            }
        }
        
        return Ok(NotificationResourceFromEntityAssembler.ToResourceFromEntity(result));
    }

    [HttpPost("{id:int}/reject")]
    public async Task<IActionResult> RejectNotification(int id)
    {
        var result = await commandService.Handle(new RejectNotificationCommand(id));
        if (result == null) return NotFound(new { message = "Notificación no encontrada" });
        
        return Ok(NotificationResourceFromEntityAssembler.ToResourceFromEntity(result));
    }
}
