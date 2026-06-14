namespace Kipu.API.Document.Interfaces.REST.Resources;

public record DocumentResource(
    string Id,
    string Type,
    bool IsSigned,
    string? DigitalSignatureToken,
    DateTime Deadline,
    string ProjectId,
    List<DocumentParticipantResource> Participants
);