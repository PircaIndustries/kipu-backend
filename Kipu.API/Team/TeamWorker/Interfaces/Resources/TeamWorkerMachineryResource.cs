using System.ComponentModel.DataAnnotations;

namespace Kipu.API.Team.TeamWorker.Interfaces.Resources;

public record TeamWorkerMachineryResource(
    [Required]
    string MachineryId, 

    [Required]
    [MaxLength(200)]
    string FullName
);