using Kipu.API.Document.Domain.Model.ValueObjects;

namespace Kipu.API.Document.Domain.Model.Aggregates;

public partial class Document
{
    public DocumentId Id { get; private set; } 
    public string Type { get; private set; }
    public bool IsSigned { get; private set; }
    public string? DigitalSignatureToken { get; private set; }
    public DateTime Deadline { get; private set; }
    public string ProjectId { get; private set; }
    

    private readonly List<DocumentParticipant> _participants = new();
    public IReadOnlyCollection<DocumentParticipant> Participants => _participants.AsReadOnly();

    protected Document() 
    { 
        Type = string.Empty;
        ProjectId = string.Empty;
    }

    public Document(string type, DateTime deadline, string projectId)
    {
        Id = new DocumentId(); 
        Type = type;
        Deadline = deadline;
        ProjectId = projectId;
        IsSigned = false; 
    }

    public void AddParticipant(string teamUserId, string fullName)
    {
        if (_participants.Any(p => p.TeamUserId == teamUserId))
            throw new InvalidOperationException("This user is already a document's participant");

        _participants.Add(new DocumentParticipant(teamUserId, fullName));
    }

    public void SignDocument(string digitalToken)
    {
        IsSigned = true;
        DigitalSignatureToken = digitalToken;
    }
}