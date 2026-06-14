using System.ComponentModel.DataAnnotations;

namespace Kipu.API.Document.Interfaces.REST.Resources;

public record SignDocumentRequest(
    [Required(ErrorMessage = "Team User ID is required.")]
    string TeamUserId,

    [Required(ErrorMessage = "Full name is required to sign the document.")]
    [MaxLength(150, ErrorMessage = "Full name cannot exceed 150 characters.")]
    string FullName
);