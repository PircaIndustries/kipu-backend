namespace Kipu.API.Document.Interfaces.REST.Resources;

public record DocumentParticipantResource(
    string TeamUserId, 
    string FullName
);