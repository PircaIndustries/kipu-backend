using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Team.TeamUser.Interfaces.REST.Resources;

[SwaggerSchema(Description = "Payload to update a team user role")]
public record UpdateTeamUserRoleResource(
    [Required] string Role
);
