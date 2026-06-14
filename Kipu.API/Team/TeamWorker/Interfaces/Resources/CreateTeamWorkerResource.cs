using System.ComponentModel.DataAnnotations;

namespace Kipu.API.Team.TeamWorker.Interfaces.Resources;

public record CreateTeamWorkerResource(
    [Required(ErrorMessage = "DNI is required.")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "DNI must be exactly 8 characters long.")]
    [RegularExpression("^[0-9]*$", ErrorMessage = "DNI must contain only numbers.")]
    string Dni, 

    [Required(ErrorMessage = "Full name is required.")]
    [MaxLength(150, ErrorMessage = "Full name cannot exceed 150 characters.")]
    string FullName, 

    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(150, ErrorMessage = "Last name cannot exceed 150 characters.")]
    string LastName, 

    [Required(ErrorMessage = "Role is required.")]
    [MaxLength(50, ErrorMessage = "Role cannot exceed 50 characters.")]
    string Role, 

    [Required(ErrorMessage = "Project ID is required.")]
    string ProjectId, 

    IEnumerable<TeamWorkerMachineryResource> Machineries
);