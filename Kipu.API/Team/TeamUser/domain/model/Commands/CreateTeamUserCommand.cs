namespace Kipu.API.Team.TeamUser.domain.model.Commands;

public record CreateTeamUserCommand(string FullName, string Email, string Role, string ProjectId);