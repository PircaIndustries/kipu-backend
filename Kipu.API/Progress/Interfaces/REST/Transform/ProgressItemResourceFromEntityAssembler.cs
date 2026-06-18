using Kipu.API.Progress.Domain.Model.Aggregates;
using Kipu.API.Progress.Interfaces.REST.Resources;

namespace Kipu.API.Progress.Interfaces.REST.Transform;

public static class ProgressItemResourceFromEntityAssembler
{
    public static ProgressItemResource ToResourceFromEntity(ProgressItem entity)
    {
        return new ProgressItemResource(entity.Id, entity.ProjectId, entity.TaskName.Value, entity.PlannedPercentage, entity.ActualPercentage);
    }
}