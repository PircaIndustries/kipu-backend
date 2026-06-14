using System.ComponentModel.DataAnnotations;

namespace Kipu.API.Team.TeamUser.Interfaces.REST.Resources;

public record CreateTeamUserResource(
    [Required(ErrorMessage = "Full name is required.")]
    [MaxLength(150, ErrorMessage = "Full name cannot exceed 150 characters.")]
    string FullName, 

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [MaxLength(100, ErrorMessage = "Email cannot exceed 100 characters.")]
    string Email, 

    [Required(ErrorMessage = "Role is required.")]
    [MaxLength(50, ErrorMessage = "Role cannot exceed 50 characters.")]
    string Role, 

    [Required(ErrorMessage = "Project ID is required.")]
    string ProjectId
);