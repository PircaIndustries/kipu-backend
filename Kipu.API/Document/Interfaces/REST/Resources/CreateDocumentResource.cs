using System.ComponentModel.DataAnnotations;

namespace Kipu.API.Document.Interfaces.REST.Resources;

public record CreateDocumentParticipantResource(string TeamUserId, string FullName);

public record CreateDocumentResource(
    [Required(ErrorMessage = "Document type is required.")]
    [MaxLength(100, ErrorMessage = "Document type cannot exceed 100 characters.")]
    string Type,

    [Required(ErrorMessage = "Deadline is required.")]
    DateTime Deadline,

    [Required(ErrorMessage = "Project ID is required.")]
    string ProjectId,
        
    IEnumerable<CreateDocumentParticipantResource> Participants
);