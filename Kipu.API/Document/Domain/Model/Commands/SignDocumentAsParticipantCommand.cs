namespace Kipu.API.Document.Domain.Model.Commands;

public record SignDocumentAsParticipantCommand(string DocumentId, string TeamUserId);