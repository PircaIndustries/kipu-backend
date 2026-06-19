using System.ComponentModel.DataAnnotations;

namespace Kipu.API.Team.TeamUser.Interfaces.REST.Resources;

public record CreateTeamUserResource(
    [Required]
    [MaxLength(150)]
    string FullName, 

    [Required]
    [EmailAddress]
    [MaxLength(100)]
    string Email, 

    [Required]
    [MaxLength(50)]
    string Role, 

    [Required]
    string ProjectId
);