using System.ComponentModel.DataAnnotations;

namespace Kipu.API.Document.Interfaces.REST.Resources;

public record SendSignCodeRequest(
    [Required]
    [MaxLength(200)]
    string Email
);
