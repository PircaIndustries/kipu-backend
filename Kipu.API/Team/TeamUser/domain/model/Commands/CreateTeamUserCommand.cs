using Kipu.API.Team.TeamUser.domain.model.ValueObjects;

namespace Kipu.API.Team.TeamUser.domain.model.Commands;

public record CreateTeamUserCommand(int UserId, string FullName, Email Email, string Role, string ProjectId);
