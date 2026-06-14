using Kipu.API.Team.TeamWorker.Domain.Model.Commands;
using Kipu.API.Team.TeamWorker.Interfaces.Resources;

namespace Kipu.API.Team.TeamWorker.Interfaces.Transform;

public static class CreateTeamWorkerCommandFromResourceAssembler
{
    public static CreateTeamWorkerCommand ToCommandFromResource(CreateTeamWorkerResource resource)
    {
        var machineries = resource.Machineries.Select(m => 
            new TeamWorkerMachineryItem(m.MachineryId, m.FullName)
        ).ToList();

        return new CreateTeamWorkerCommand(
            resource.Dni,
            resource.FullName,
            resource.Role,
            resource.ProjectId,
            machineries
        );
    }
}