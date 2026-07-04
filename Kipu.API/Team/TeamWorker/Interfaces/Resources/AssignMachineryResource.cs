using System.ComponentModel.DataAnnotations;

namespace Kipu.API.Team.TeamWorker.Interfaces.Resources;

public record AssignMachineryResource(
    [Required]
    string MachineryId, 

    [Required]
    [MaxLength(200)]
    string FullName
);