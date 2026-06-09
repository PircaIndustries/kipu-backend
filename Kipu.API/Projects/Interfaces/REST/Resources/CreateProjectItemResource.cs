namespace Kipu.API.Projects.Interfaces.REST.Resources;

public record CreateProjectItemResource(
    string Name,
    DateTime StartDate,
    DateTime EndDate
);
