namespace Kipu.API.Notifications.Domain.Model.Aggregates;

public class Notification
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string RelatedProjectId { get; set; } = string.Empty;
    public string Payload { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending"; // Pending, Accepted, Rejected
    public bool IsRead { get; set; } = false;
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public Notification() { }

    public Notification(string userId, string message, string type, string relatedProjectId, string payload)
    {
        UserId = userId;
        Message = message;
        Type = type;
        RelatedProjectId = relatedProjectId;
        Payload = payload;
    }
}
