namespace Kipu.API.Notifications.Domain.Model.Commands;
public record CreateNotificationCommand(string UserId, string Message, string Type, string RelatedProjectId, string Payload);
