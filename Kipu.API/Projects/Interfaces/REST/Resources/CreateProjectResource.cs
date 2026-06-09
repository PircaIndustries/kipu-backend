namespace Kipu.API.Projects.Interfaces.REST.Resources;

public record CreateProjectResource(
    string Name,
    string Location,
    double Budget,
    string Description = "",
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    string Image = "",
    int Members = 1,
    int Rnc = 0,
    int Pending = 0,
    string Status = "Planificación",
    string StatusJustification = "Proyecto creado e iniciado."
);
