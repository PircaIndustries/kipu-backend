namespace Kipu.API.Projects.Interfaces.REST.Resources;

public record ProjectResource(
    int Id,
    string Name,
    string Description,
    string Location,
    double Budget,
    int Progress,
    string Status,
    string StatusJustification,
    string Image,
    int Members,
    int Rnc,
    int Pending,
    DateTime? StartDate,
    DateTime? EndDate
);
