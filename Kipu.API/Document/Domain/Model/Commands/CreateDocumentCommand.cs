namespace Kipu.API.Document.Domain.Model.Commands;

public record DocumentParticipantItem(string TeamUserId, string FullName);

public record CreateDocumentCommand(
    string Type, 
    DateTime Deadline, 
    string ProjectId, 
    IEnumerable<DocumentParticipantItem> Participants
);