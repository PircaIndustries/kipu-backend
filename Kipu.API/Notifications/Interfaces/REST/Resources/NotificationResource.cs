namespace Kipu.API.Notifications.Interfaces.REST.Resources;

public record NotificationResource(int Id, string UserId, string Message, string Type, string RelatedProjectId, string Status, bool IsRead, string CreatedAt);
