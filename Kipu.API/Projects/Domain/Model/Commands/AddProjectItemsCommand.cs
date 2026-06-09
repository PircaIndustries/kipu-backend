namespace Kipu.API.Projects.Domain.Model.Commands;

public record ProjectItemRequest(string Name, DateTime StartDate, DateTime EndDate);

public record AddProjectItemsCommand(int ProjectId, List<ProjectItemRequest> Items);
