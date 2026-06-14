namespace Kipu.API.Team.TeamUser.Interfaces.REST.Resources;

public record TeamUserResource(string Id, string FullName, string Email, string Role, bool IsActive, string ProjectId);