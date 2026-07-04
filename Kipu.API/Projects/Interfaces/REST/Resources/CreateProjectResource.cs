namespace Kipu.API.Projects.Interfaces.REST.Resources;

public record CreateProjectResource(
    string Name,
    string Location,
    double Budget,
    string CreatorUserId,
    string CreatorEmail,
    string CreatorFullName,
    string Description = "",
    DateTime? StartDate = null,
    DateTime? EndDate = null,
    string Image = "",
    int Members = 1,
    int Rnc = 0,
    int Pending = 0,
    string Status = "Planificaci�n",
    string StatusJustification = "Proyecto creado e iniciado."
);
