using System.ComponentModel.DataAnnotations;

namespace Kipu.API.Document.Interfaces.REST.Resources;

public record CreateDocumentParticipantResource(string TeamUserId, string FullName);

public record CreateDocumentResource(
    [Required]
    [MaxLength(100)]
    string Type,

    [Required]
    DateTime Deadline,

    [Required]
    string ProjectId,
        
    IEnumerable<CreateDocumentParticipantResource> Participants
);