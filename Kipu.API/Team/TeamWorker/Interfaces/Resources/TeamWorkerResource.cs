namespace Kipu.API.Team.TeamWorker.Interfaces.Resources;

public record TeamWorkerResource(
    string Id,
    string Dni,
    string FullName,
    string Role,
    bool IsActive,
    string ProjectId,
    List<TeamWorkerMachineryResource> Machineries
);