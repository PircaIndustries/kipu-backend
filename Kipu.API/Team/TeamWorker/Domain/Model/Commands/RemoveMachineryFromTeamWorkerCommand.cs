namespace Kipu.API.Team.TeamWorker.Domain.Model.Commands;

public record RemoveMachineryFromTeamWorkerCommand(
    string TeamWorkerId, 
    string MachineryId
);