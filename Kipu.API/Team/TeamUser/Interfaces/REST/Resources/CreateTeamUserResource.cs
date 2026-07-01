using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Team.TeamUser.Interfaces.REST.Resources;

[SwaggerSchema(Description = "Payload to create a new team user")]
public record CreateTeamUserResource(
    [Required] int UserId,
    [Required] string FullName,
    [Required] string Email,
    [Required] string Role,
    [Required] string ProjectId
);
