namespace Kipu.API.Team.TeamWorker.Domain.Model.Commands;

public record TeamWorkerMachineryItem(string MachineryId, string FullName);

public record CreateTeamWorkerCommand(
    string Dni, 
    string FullName, 
    string Role, 
    string ProjectId, 
    IEnumerable<TeamWorkerMachineryItem> Machineries
);