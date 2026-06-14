namespace Kipu.API.Document.Domain.Model.Aggregates;

public class DocumentParticipant
{
    public int Id { get; private set; } 
    public string TeamUserId { get; private set; } 
    public string FullName { get; private set; } 
    
    public DateTimeOffset? SignedAt { get; private set; } 

    protected DocumentParticipant() 
    { 
        TeamUserId = string.Empty;
        FullName = string.Empty;
    }

    public DocumentParticipant(string teamUserId, string fullName)
    {
        TeamUserId = teamUserId;
        FullName = fullName;
        SignedAt = null; 
    }

    public void MarkAsSigned()
    {
        SignedAt = DateTimeOffset.UtcNow;
    }
}