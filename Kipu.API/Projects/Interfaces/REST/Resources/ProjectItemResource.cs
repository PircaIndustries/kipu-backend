namespace Kipu.API.Projects.Interfaces.REST.Resources;

public record ProjectItemResource(
    int Id,
    string Name,
    DateTime StartDate,
    DateTime EndDate,
    int ProjectId
);
