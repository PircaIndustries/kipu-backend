using System.ComponentModel.DataAnnotations;

namespace Kipu.API.Document.Interfaces.REST.Resources;

public record SignDocumentRequest(
    [Required]
    string TeamUserId,

    [Required]
    [MaxLength(150)]
    string FullName,

    [Required]
    [MaxLength(6)]
    string Code,

    [Required]
    [MaxLength(200)]
    string Email
);