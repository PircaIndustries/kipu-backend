using System.ComponentModel.DataAnnotations;

namespace Kipu.API.Projects.Interfaces.REST.Resources;

public record UpdateProjectStatusResource(
    [Required] string Status,
    [Required] string StatusJustification
);
