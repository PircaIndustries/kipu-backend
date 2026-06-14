using Kipu.API.Team.TeamWorker.Interfaces.Resources;

namespace Kipu.API.Team.TeamWorker.Interfaces.Transform;

public static class TeamWorkerResourceFromEntityAssembler
{
    public static TeamWorkerResource ToResourceFromEntity(Domain.Model.Aggregates.TeamWorker entity)
    {
        var machineries = entity.Machineries.Select(m => 
            new TeamWorkerMachineryResource(m.MachineryId, m.FullName)
        ).ToList();

        return new TeamWorkerResource(
            entity.Id.Value,
            entity.Dni,
            entity.FullName,
            entity.Role,
            entity.IsActive,
            entity.ProjectId,
            machineries
        );
    }
}