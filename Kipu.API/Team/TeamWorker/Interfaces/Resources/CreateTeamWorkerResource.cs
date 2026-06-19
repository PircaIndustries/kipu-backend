using System.ComponentModel.DataAnnotations;

namespace Kipu.API.Team.TeamWorker.Interfaces.Resources;

public record CreateTeamWorkerResource(
    [Required]
    [StringLength(8, MinimumLength = 8)]
    [RegularExpression("^[0-9]*$")]
    string Dni, 

    [Required]
    [MaxLength(150)]
    string FullName, 

    [Required]
    [MaxLength(150)]
    string LastName, 

    [Required]
    [MaxLength(50)]
    string Role, 

    [Required]
    string ProjectId, 

    IEnumerable<TeamWorkerMachineryResource> Machineries
);