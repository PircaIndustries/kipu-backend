using Kipu.API.Notifications.Domain.Model.Aggregates;
using Kipu.API.Notifications.Interfaces.REST.Resources;

namespace Kipu.API.Notifications.Interfaces.REST.Transform;

public static class NotificationResourceFromEntityAssembler
{
    public static NotificationResource ToResourceFromEntity(Notification entity)
    {
        return new NotificationResource(
            entity.Id,
            entity.UserId,
            entity.Message,
            entity.Type,
            entity.RelatedProjectId,
            entity.Status,
            entity.IsRead,
            entity.CreatedAt.ToString("o")
        );
    }
}
