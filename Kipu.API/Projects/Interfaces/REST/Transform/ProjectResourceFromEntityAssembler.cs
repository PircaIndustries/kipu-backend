using Kipu.API.Projects.Domain.Model.Aggregates;
using Kipu.API.Projects.Interfaces.REST.Resources;

namespace Kipu.API.Projects.Interfaces.REST.Transform;

public static class ProjectResourceFromEntityAssembler
{
    public static ProjectResource ToResourceFromEntity(Project entity)
    {
        return new ProjectResource(
            entity.Id,
            entity.Name,
            entity.Description,
            entity.Location,
            entity.Budget,
            entity.Progress,
            entity.Status,
            entity.StatusJustification,
            entity.Image,
            entity.Members,
            entity.Rnc,
            entity.Pending,
            entity.StartDate,
            entity.EndDate
        );
    }
}
