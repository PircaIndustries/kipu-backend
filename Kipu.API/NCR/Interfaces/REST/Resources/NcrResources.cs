using System.ComponentModel.DataAnnotations;

namespace Kipu.API.Ncr.Interfaces.REST.Resources;

public record CreateNcrResource(
    [Required] string Title,
    [Required] string Description,
    [Required] string TaskName,
    [Required] int Severity,
    [Required] int ProjectId
);

public record NcrResource(
    int Id,
    string Title,
    string Description,
    string TaskName,
    string Severity,
    int ProjectId,
    string Status,
    string RootCause,
    string CorrectiveAction
);