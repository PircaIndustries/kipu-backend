using Kipu.API.Team.TeamUser.domain.model.Commands;
using Kipu.API.Team.TeamUser.Interfaces.REST.Resources;

namespace Kipu.API.Team.TeamUser.Interfaces.REST.Transform;

public static class CreateTeamUserCommandFromResourceAssembler
{
    public static CreateTeamUserCommand ToCommandFromResource(CreateTeamUserResource resource)
    {
        return new CreateTeamUserCommand(
            resource.FullName,
            resource.Email,
            resource.Role,
            resource.ProjectId);
    }
}