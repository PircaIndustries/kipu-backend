using Kipu.API.Team.TeamUser.domain.model.Commands;

namespace Kipu.API.Team.TeamUser.application.Services;

public interface ITeamUserCommandService
{
    Task<domain.model.Aggregates.TeamUser?> Handle(CreateTeamUserCommand command);
    Task<domain.model.Aggregates.TeamUser?> Handle(ActivateTeamUserCommand command);
    Task<domain.model.Aggregates.TeamUser?> Handle(DeactivateTeamUserCommand command);
}