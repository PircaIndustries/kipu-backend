namespace Kipu.API.Team.TeamWorker.Domain.Model.Commands;

public record AssignMachineryToTeamWorkerCommand(
    string TeamWorkerId, 
    string MachineryId, 
    string FullName
);