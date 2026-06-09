namespace Kipu.API.Projects.Domain.Model.Commands;

public record UpdateProjectStatusCommand(int ProjectId, string Status, string Justification);
