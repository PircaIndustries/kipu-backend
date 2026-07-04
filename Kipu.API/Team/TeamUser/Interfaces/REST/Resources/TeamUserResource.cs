using Swashbuckle.AspNetCore.Annotations;

namespace Kipu.API.Team.TeamUser.Interfaces.REST.Resources;

[SwaggerSchema(Description = "A team user resource")]
public record TeamUserResource(
    string Id,
    int? UserId,
    string FullName,
    string Email,
    string Role,
    bool IsActive,
    string ProjectId
);
