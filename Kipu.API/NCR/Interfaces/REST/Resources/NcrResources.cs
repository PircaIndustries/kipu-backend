using System.ComponentModel.DataAnnotations;

namespace Kipu.API.Ncr.Interfaces.REST.Resources;

public record CreateNcrResource(
    [Required] string Title,
    [Required] string Description,
    [Required] string Severity,
    [Required] int ProjectId,
    [Required] string Speciality
);

public record NcrResource(
    int Id,
    string Title,
    string Description,
    string Severity,
    int ProjectId,
    string Speciality
);